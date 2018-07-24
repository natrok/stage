texture d_State0;
texture d_State1;

//Need for knowing velocity
texture d_color_ramp;
texture d_wind;

float d_particles_res;
float2 d_resScreen;

float2 d_wind_min;
float2 d_wind_max;

float d_contraste;
bool d_waves;

sampler d_windSampler : register(s0) =
sampler_state { Texture = <d_wind>; };

sampler d_particlesSampler0 : register(s1) =
sampler_state {Texture = <d_State0>;};

sampler d_particlesSampler1 : register(s2) =
sampler_state { Texture = <d_State1>; };

sampler d_color_rampSampler : register(s3) =
sampler_state { Texture = <d_color_ramp>; };

//Fix shader
float d_fadeTableau[10];
int d_ParticulesByBlock;
int d_delta;

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
	
	VSOut output;	
	
	//x index position in state texture <0,255>,  zw .. vertex position in particle flags
	float index = position.x;
	float fadeValue;	
	float4 tc = float4(
		frac(index / d_particles_res),
		floor(index / d_particles_res) / d_particles_res,
		0.0, 0.0);

	int indexBlock = floor(index / d_ParticulesByBlock);
	fadeValue = d_fadeTableau[(indexBlock + d_delta) % 10];
	

	output.fade = fadeValue;

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

	float sizeP = 4.0f; 

	float2 sizeOfParticle = float2(sizeP / d_resScreen.x , sizeP / d_resScreen.y);
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
		if (d > maxDx || d < 0.0002)
			pos.x += 5.0;// bad particle! move away!
	}else
		if (d > maxDx) { 			
			pos.x = 5.0f;	// bad particle! move away!		
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

	float opacity = d_contraste  + (speed * (1.0 - d_contraste));

	float fade = In.fade;
	float4 res = float4(ColorRes.xyz, 1.0);

	float4 color = res * opacity;
	color = float4(color.xyz, fade);	

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




