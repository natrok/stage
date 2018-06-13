//----------------------------
//-------||VARIABLES||--------
//----------------------------

//Textures
texture d_wind;
texture d_particles;
texture d_color_ramp;

//wind variables
float  d_particles_resolution;
float2 d_wind_min;
float2 d_wind_max;

//SAMPLERS
sampler d_windSampler : register(s0) =
sampler_state
{	Texture = <d_wind>;};

sampler d_particlesSampler : register(s1) =
sampler_state
{	Texture = <d_particles>;};

sampler d_color_rampSampler : register(s2) =
sampler_state
{	Texture = <d_color_ramp>;};

//-------------------------
struct VSOut //Vertex Output
{
	float4 positionParticle : POSITION;
	float PointSize : PSIZE0;
	float2 v_particle_pos : TEXCOORD0;
};

//----------------------------
//-----||VERTEX SHADER||------
//----------------------------


VSOut main_VS(float a_index: POSITION ) //Main_Vs recu les buffer values in this case index
{
	VSOut output;

	/*
	texture coordinates in a 2D texture are between 0 and 1.
	vec2 tcoord = vec2(gl_FragCoord.x / width, gl_FragCoord.y);
	that's why we devise by d_particles_resolution
	*/
	

	
	float4 color = tex2Dlod(d_particlesSampler,
		float4(
			frac(a_index / d_particles_resolution),
			floor(a_index / d_particles_resolution) / d_particles_resolution,
			0.0,
			0.0));
			
	//Use color for finding positions
	output.v_particle_pos = float2(
		color.r / 255.0 + color.b,
		color.g / 255.0 + color.a
		);

	//safe position in ecran Values in rg 0,1 || Y like negative value
	output.positionParticle = float4(2.0 * output.v_particle_pos.x - 1.0, 1.0 - 2.0 * output.v_particle_pos.y, 0.0, 1.0); //set position in rg channels
	output.PointSize = 3.0;//Size of particle


	return output;
}

//----------------------------
//----||FRAGMENT SHADER||-----
//----------------------------
float4 main_FG(VSOut IN): COLOR0
{
	//VSOut 
	float4 ColorRes;
	//calculate velocity
	/*encode velocity in a positive range*/
	float2 velocity = lerp(d_wind_min, d_wind_max, tex2D(d_windSampler, IN.v_particle_pos).rg);
	float speed = length(velocity) / length(d_wind_max);

	// color ramp is encoded in a 16x16 texture
	float2 ramp_pos = float2(
		frac(16.0 * speed), // X
		floor(16.0 * speed) / 16.0 //Y
	);

	ColorRes = tex2D(d_color_rampSampler, ramp_pos);
	return ColorRes;
}

technique TDefault
{
	pass P0
	{
		VertexShader = compile vs_3_0 main_VS();
		PixelShader = compile ps_3_0 main_FG();
	}
}