texture d_State0;
texture d_State1;
texture d_color_ramp;
//Need for knowing velocity
texture d_wind;

float d_particles_res;
float2 d_resScreen;

float2 d_wind_min;
float2 d_wind_max;

float d_opacity_min;
bool d_waves;

sampler d_windSampler : register(s0) =
sampler_state { Texture = <d_wind>; };

sampler d_particlesSampler0 : register(s1) =
sampler_state {Texture = <d_State0>;};

sampler d_particlesSampler1 : register(s2) =
sampler_state { Texture = <d_State1>; };

sampler d_color_rampSampler : register(s3) =
sampler_state { Texture = <d_color_ramp>; };


//----------------------------
//-----||VERTEX SHADER||------
//----------------------------
struct VSOut //Vertex shader out
{
	float4 position : POSITION0;	
	float fade : COLOR0;
	float2 v_particle_pos : TEXCOORD0;
};

float2 Translate;

VSOut main_VS(float4 position : POSITION) //intended use of the variable
{
	//x index position in state texture <0,255>, y fade value; zw .. vertex position in particle flags
	VSOut output;	
	output.fade = position.y;
	
	float index = position.x; 	
	float4 tc = float4(
		frac(index / d_particles_res),
		floor(index / d_particles_res) / d_particles_res,
		0.0, 0.0);

	float4 posColorA = tex2Dlod(d_particlesSampler0, tc);
	float4 posColorB = tex2Dlod(d_particlesSampler1, tc);

	//Decode Position Particles
	float2 posA = float2(
		posColorA.r / 255.0 + posColorA.b,
		posColorA.g / 255.0 + posColorA.a
	);

	float2 posB = float2(
		posColorB.r / 255.0 + posColorB.b,
		posColorB.g / 255.0 + posColorB.a
		);	

	//Set coordinates of texture <0, 1> space
	output.v_particle_pos = posB;

	posA = float2(2.0 * posA.x - 1.0, 1.0 - 2.0 * posA.y ); // particle position in <-1.0,1.0> space
	posB = float2(2.0 * posB.x - 1.0, 1.0 - 2.0 * posB.y ); // last particle position <-1.0,1.0> space

	float2 sizeOfParticle = float2(4.0f / d_resScreen.x / 2.0, 4.0f / d_resScreen.y / 2.0);
	float2 dirF = posA - posB ;
	float2 dirFN = normalize(dirF); // normalized forward direction ( from B to A )
	float d = length(dirF); // d can be used for alpha from speed
	float2 dirRN = float2(dirFN.y, -dirFN.x); // perpendicular direction (right from dirFN
	float2 pos = lerp(posA, posB, position.w);// select posA or posB for extend quad	
	pos += dirRN * (position.zz * sizeOfParticle);

	float maxDx = 0.01f;
	
	if(d_waves){
		sizeOfParticle = float2(30 / d_resScreen.x / 2.0, 10.0f / d_resScreen.y / 2.0);
		pos += dirRN * (position.zz * sizeOfParticle);
		//color.xyz = float3(0.5, 0.67, 1.0);
		if (d > maxDx || d < 0.0002)
			pos.x += 5.0;// bad particle! move away!
	}else
		if (d > maxDx) {
 			// bad particle! move away!
			pos.x = 5.0f;			
		}

	
	output.position = float4(pos.xy, 0.0, 1.0);
	return output;
}

//----------------------------
//----||FRAGMENT SHADER||-----
//----------------------------
float4 main_FG(VSOut In): COLOR0
{
	//VSOut 
	float4 ColorRes;
	
	float2 velocity = lerp(d_wind_min, d_wind_max, tex2D(d_windSampler, In.v_particle_pos).rg);
	float speed = length(velocity) / length(d_wind_max); //value de 0 to 1;
	
	// color ramp is encoded in a 16x16 texture
	float2 ramp_pos = float2(
		frac(16.0 * speed), // X
		floor(16.0 * speed) / 16.0 //Y
		);
	ColorRes = tex2D(d_color_rampSampler, ramp_pos);
	ColorRes = float4(1.0, 1.0, 1.0, 1.0);

	float opacity = d_opacity_min + (speed * (1.0 - d_opacity_min)) / 1.0f;
	float fade = In.fade;
	float4 res = float4(ColorRes.xyz, 1.0);
	float4 color = res * opacity;
	color = float4(color.xyz, fade);	
	//fade so doucement
	//float4 aa = float4(fade, fade, fade, fade);
	//aa = float4(floor(255.0 * aa * fade) / 255.0);
	//color = res * aa;	

	if(d_waves)
		color = float4(0.5, 0.67, 1.0,1.0);
	
	

	return color;
}

technique TDefault
{
	pass P0
	{		
		VertexShader = compile vs_3_0 main_VS();
		PixelShader = compile ps_3_0 main_FG();
	}
}

