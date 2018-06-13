//----------------------------
//-------||VARIABLES||--------
//----------------------------

//Textures
texture d_wind;
texture d_particles;
float2 d_wind_resolution;

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



//inout allow to read and modify
void updatePosition(inout float2 p, inout float2 v) {
	p += v;//position - velocity
}


//-------------------------
struct VSOut //Vertex Output
{
	float4 position: POSITION;
	float2 v_particle_pos : TEXCOORD0;
};

//----------------------------
//-----||VERTEX SHADER||------
//----------------------------
VSOut main_VS(float2 a_pos : POSITION)
{
	VSOut output;
	output.v_particle_pos = a_pos;
	output.position = float4(1.0 - 2.0 * a_pos, 0, 1);
	return output;
}

float2 lookup_wind(float2 uv) {
	float2 px = 1.0 / d_wind_resolution;
	float2 vc = (floor(uv * d_wind_resolution)) * px;
	return tex2D(d_windSampler, vc).rg;
}

// w = canvas.width, h = canvas.height;
// var scale = Math.floor(Math.pow(Particles.BASE, 2) / Math.max(w, h) / 3);
						//255^2 / height / 3
// const float BASE = 255.0;
// const float OFFSET = BASE * BASE / 2.0;

//----------------------------
//----||FRAGMENT SHADER||-----
//----------------------------
float4 main_FG(VSOut IN): COLOR0
{
	//VSOut 
	float4 UpdateRes;

	/*Decode Particle Position*/
	float4 psample = tex2D(d_particlesSampler, IN.v_particle_pos);
	float2 p = float2(
		psample.r / 255.0 + psample.b,
		psample.g / 255.0 + psample.a);
	
	float2 v = lookup_wind(p);
	float2 result;

	/*Calculate new Position*/
	updatePosition(p, v);
	result = frac(p);

	/*Encode nouvelle Position*/
	float4 res = float4(
		frac(result * 255.0),
		floor(result * 255.0) / 255.0);

	return res;
}


//------------------------
//----||COMPILATION||-----
//------------------------
technique TDefault
{
	pass P0
	{
		VertexShader = compile vs_3_0 main_VS();
		PixelShader = compile ps_3_0 main_FG();
	}
}