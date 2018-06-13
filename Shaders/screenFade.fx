//----------------------------
//-------||VARIABLES||--------
//----------------------------
texture d_screen;
float d_opacity;
float d_miPixX;
float d_miPixY;

sampler d_screenSampler =
sampler_state
{
	Texture = <d_screen>;
};


//----------------------------
//-----||VERTEX SHADER||------
//----------------------------
struct VSOut //Vertex shader out
{
	float4 Position : POSITION;
	float2 v_tex_pos : TEXCOORD0;
};


VSOut main_VS(float2 a_pos : POSITION) //intended use of the variable
{
	VSOut output;
	output.v_tex_pos = float2(a_pos.x + d_miPixX, 1.0 - a_pos.y + d_miPixY);
	output.Position = float4(1.0 - 2.0 * a_pos, 0, 1);

	return output;
}

//----------------------------
//----||FRAGMENT SHADER||-----
//----------------------------
float4 main_FG(VSOut In): COLOR0
{
	float4 color = tex2D(d_screenSampler, 1.0 - In.v_tex_pos);
	// a hack to guarantee opacity fade out even with a value close to 1.0
	float4 res = float4(floor(255.0 * color * d_opacity) / 255.0);
	return res;
}

technique TDefault
{
	pass P0
	{
		VertexShader = compile vs_3_0 main_VS();
		PixelShader = compile ps_3_0 main_FG();
	}
}