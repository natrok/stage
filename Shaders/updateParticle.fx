texture d_wind;
texture d_particles;

float2 d_wind_resolution;
float2 d_wind_min;
float2 d_wind_max;
float d_rand_seed;
float d_speed_factor;
float d_drop_rate;
float d_drop_rate_bump;
bool d_voisin;

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

struct VSOut
{
	float4 position : POSITION;
	float2 v_particle_pos : TEXCOORD0;
};

VSOut MainVS(float2 a_pos : POSITION)
{
	VSOut output;

	output.v_particle_pos = a_pos;
	output.position = float4(1.0 - 2.0 * a_pos, 0, 1);
	return output;
}


// pseudo-random generator
float3 rand_constants = float3(12.9898, 78.233, 4375.85453);
float rand(float2 co)
{
	float t = dot(rand_constants.xy, co);
	return frac(sin(t) * (rand_constants.z + t));
}

float2 lookup_wind_px(float2 uv) {
	float2 px = 1.0 / d_wind_resolution;
	float2 vc = (floor(uv * d_wind_resolution)) * px;
	return tex2D(d_windSampler, vc).rg;
}

// wind speed lookup; use manual bilinear filtering based on 4 adjacent pixels for smooth interpolation
float2 lookup_wind(float2 uv)
{
	// return texture2D(d_wind, uv).rg; // lower-res hardware filtering
	float2 px = 1.0 / d_wind_resolution;
	float2 vc = (floor(uv * d_wind_resolution)) * px;
	float2 f = frac(uv * d_wind_resolution);
	float2 tl = tex2D(d_windSampler, vc).rg;
	float2 tr = tex2D(d_windSampler, vc + float2(px.x, 0)).rg;
	float2 bl = tex2D(d_windSampler, vc + float2(0, px.y)).rg;
	float2 br = tex2D(d_windSampler, vc + px).rg;
	return lerp(lerp(tl, tr, f.x), lerp(bl, br, f.x), f.y);
}

void updatePosition(inout float2 pos, inout float2 velocity) {

	float distortion = cos(radians(pos.y * 180.0 - 90.0));
	float2 offset = float2(velocity.x / distortion, -velocity.y) * 0.0001 * d_speed_factor;//changement
																						   // update particle position
	pos = frac(1.0 + pos + offset); //pos + offset = desplacement
}



float4 MainPS(VSOut In) : COLOR0
{
	float4 color = tex2D(d_particlesSampler, In.v_particle_pos);
	float2 pos = float2(
		color.r / 255.0 + color.b,
		color.g / 255.0 + color.a); // decode particle position from pixel RGBA
	
	float2 vent;
	if (d_voisin){
	 vent = lookup_wind(pos);
	}
	else {
	 vent = lookup_wind_px(pos);
	}

	float2 velocity = lerp(d_wind_min, d_wind_max, vent);

	//float2 velocity = lerp(d_wind_min, d_wind_max, lookup_wind(pos));
	float speed_t = length(velocity) / length(d_wind_max);

	// take EPSG:4236 distortion into account for calculating where the particle moved
	//float distortion = cos(radians(pos.y * 180.0 - 90.0));
	//float2 offset = float2(velocity.x / distortion, -velocity.y) * 0.0001 * d_speed_factor;
	//
	//// update particle position, wrapping around the date line
	//pos = frac(1.0 + pos + offset);

	updatePosition(pos, velocity);

	// a random seed to use for the particle drop
	float2 seed = (pos + In.v_particle_pos) * d_rand_seed; /*Important*/

	//speed_t = 0.50f;

	// drop rate is a chance a particle will restart at random position, to avoid degeneration
	float drop_rate = d_drop_rate + speed_t * d_drop_rate_bump;
	float drop = step(1.0 - drop_rate, rand(seed)); //co,pare two values

	drop = step(1.0 - drop_rate, rand(seed));

	float2 random_pos = float2(
		rand(seed + 1.3),
		rand(seed + 2.1));
	pos = lerp(pos, random_pos, drop);

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
		PixelShader = compile ps_3_0 MainPS();
	}
}