texture d_State0;
texture d_State1;
float d_particles_res;
float2 d_resScreen;
bool d_waves;


sampler d_particlesSampler0 : register(s1) =
sampler_state {Texture = <d_State0>;};
sampler d_particlesSampler1 : register(s2) =
sampler_state { Texture = <d_State1>; };

//----------------------------
//-----||VERTEX SHADER||------
//----------------------------
struct VSOut //Vertex shader out
{
	float4 position : POSITION0;	
	float4 color : COLOR0;
};

float2 Translate;

VSOut main_VS(float4 position : POSITION) //intended use of the variable
{
	//x index position in state texture <0,255>, y fade value; zw .. vertex position in particle flags
	VSOut output;	
		
	float4 color = float4(0.69,0.69, 0.69,position.y);

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

	posA = float2(2.0 * posA.x - 1.0, 1.0 - 2.0 * posA.y ); // particle position in <-1.0,1.0> space
	posB = float2(2.0 * posB.x - 1.0, 1.0 - 2.0 * posB.y ); // last particle position <-1.0,1.0> space

	float2 sizeOfParticle = float2(05.0f / d_resScreen.x / 2.0, 10.0f / d_resScreen.y / 2.0);
	float2 dirF = posA - posB ;
	float2 dirFN = normalize(dirF); // normalized forward direction ( from B to A )
	float d = length(dirF); // d can be used for alpha from speed
	float2 dirRN = float2(dirFN.y, -dirFN.x); // perpendicular direction (right from dirFN
	float2 pos = lerp(posA, posB, position.w);// select posA or posB for extend quad	
	pos += dirRN * (position.zz * sizeOfParticle);

	float maxDx = 0.02f;
	
	if(d_waves){
		sizeOfParticle = float2(30 / d_resScreen.x / 2.0, 10.0f / d_resScreen.y / 2.0);
		pos += dirRN * (position.zz * sizeOfParticle);
		color.xyz = float3(0.5, 0.67, 1.0);
		if (d > maxDx || d < 0.0001)
			pos.x += 5.0;// bad particle! move away!
	}else
		if (d > maxDx) {
 			// bad particle! move away!
			pos.x = 5.0f;			
		}

	//output.fade = position.y;
	output.position = float4(pos.xy, 0.0, 1.0);
	output.color = color;

	return output;
}

//----------------------------
//----||FRAGMENT SHADER||-----
//----------------------------
float4 main_FG(VSOut In): COLOR0
{
	//VSOut 
	float fade = In.color.w;
	float4 res = float4(In.color.xyz, 1.0);
	float4 aa = float4(fade, fade, fade, fade);
	aa = float4(floor(255.0 * aa * fade) / 255.0);
	return res * aa;
}

technique TDefault
{
	pass P0
	{		
		VertexShader = compile vs_3_0 main_VS();
		PixelShader = compile ps_3_0 main_FG();
	}
}

