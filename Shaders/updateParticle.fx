//----------------------------
//-------||VARIABLES||--------
//----------------------------
texture d_wind;
texture d_particles;

float d_particles_res;
float d_particles_min;
float d_particles_max;

float2 d_wind_resolution;
float2 d_wind_min;
float2 d_wind_max; 

float d_rand_seed;
float d_speed_factor;

float d_miPixX;
float d_miPixY;

//SAMPLERS FOR TEXTURE
sampler d_windSampler : register(s0) =
sampler_state
{
	Texture = <d_wind>;
};

sampler d_particlesSampler : register(s1) =
sampler_state
{
	Texture = <d_particles>;
};

//Fix shader
int d_ParticulesByBlock;
int d_delta;
//----------------------------
//-----||VERTEX METHODS||-----
//----------------------------
struct VSOut
{
	float4 position : POSITION;
	float2 v_particle_pos : TEXCOORD0;
};

void updatePosition(inout float2 pos, inout float2 velocity) {
	// update particle position
	float2 offset = float2(velocity.x, -velocity.y) * 0.0001 * d_speed_factor;//changement
	pos = frac(1.0 + pos + offset); 
}

// pseudo-random generator
float3 rand_constants = float3(12.9898, 78.233, 4375.85453);
float rand(float2 co)
{
	float t = dot(rand_constants.xy, co);
	return frac(sin(t) * (rand_constants.z + t));
}

// wind speed lookup; use manual bilinear filtering based on 4 adjacent pixels for smooth interpolation
float2 lookup_wind(float2 uv)
{	
	float2 px = 1.0 / d_wind_resolution;
	float2 vc = (floor(uv * d_wind_resolution)) * px;
	float2 f = frac(uv * d_wind_resolution);
	float2 tl = tex2D(d_windSampler, vc).rg;
	float2 tr = tex2D(d_windSampler, vc + float2(px.x, 0)).rg;
	float2 bl = tex2D(d_windSampler, vc + float2(0, px.y)).rg;
	float2 br = tex2D(d_windSampler, vc + px).rg;
	return lerp(lerp(tl, tr, f.x), lerp(bl, br, f.x), f.y);
}

bool TestRange(int numberToCheck, int bottom, int top)
{
	return (numberToCheck >= bottom && numberToCheck < top);
}


//----------------------------
//-----||VERTEX SHADER||------
//----------------------------
VSOut MainVS(float2 a_pos : POSITION)
{
	VSOut output;
	output.v_particle_pos = float2((1.0 - a_pos.x) - d_miPixX, a_pos.y - d_miPixY);	
	output.position = float4(1.0 - 2.0 * a_pos, 0, 1);
	return output;
}

//------------------------------
//-----||FRAGMENT SHADER||------
//------------------------------
float4 MainPS(VSOut In , uniform bool isWave) : COLOR0
{
	float4 color = tex2D(d_particlesSampler, In.v_particle_pos);
	float2 pos = float2(
		color.r / 255.0 + color.b,
		color.g / 255.0 + color.a); // decode particle position from pixel RGBA
	
	float2 px = 1.0 / d_particles_res;
	float2 vc = (floor(In.v_particle_pos * d_particles_res)) * px;		
	
	int index = (1.0 - vc.y) * d_particles_res * d_particles_res + vc.x * d_particles_res;
	bool resetParticle = TestRange(index, d_particles_min, d_particles_max);
	
	
	float2 vent = lookup_wind(pos);
	float2 velocity = lerp(d_wind_min, d_wind_max, vent);

	if (isWave)
		velocity = lerp(d_wind_min / 4.0, d_wind_max / 4.0f, vent);
	
	// update particle position, wrapping around the date line
	updatePosition(pos, velocity);	
	
	// a random seed to use for the particle drop, it gives you a random Position of screen coordinates
	float2 seed = (pos + In.v_particle_pos) * d_rand_seed; /*Important*/
	float2 random_pos = float2(
		rand(seed + 1.3),
		rand(seed + 2.1));

	pos = lerp( pos , random_pos, resetParticle);
	// encode the new particle position back into RGBA
	float4 res = float4(
		frac(pos * 255.0),
		floor(pos * 255.0) / 255.0);
	
	return res;
}


technique Default
{
	pass P0
	{
		VertexShader = compile vs_3_0 MainVS();
		PixelShader = compile ps_3_0 MainPS(false);
	}
}

technique Wave
{
	pass P0
	{
		VertexShader = compile vs_3_0 MainVS();
		PixelShader = compile ps_3_0 MainPS( true);
	}
}