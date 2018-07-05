//----------------------------
//-------||VARIABLES||--------
//----------------------------

//Textures
texture d_wind;
texture d_wave;
texture d_particles;
texture d_color_ramp;

//wind variables
float d_particles_resolution;
float d_speed_factor;
float2 d_wind_min;
float2 d_wind_max;
float d_lifeTime;
float d_maxLife;



bool d_PointSpriteEnable;

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

//SAMPLERS
sampler d_waveSampler : register(s3) =
sampler_state
{
	Texture = <d_wave>;
};


//-------------------------
struct VSOut //Vertex Output
{
	float4 positionParticle : POSITION;
	float PointSize : PSIZE0;
	float2 v_particle_pos : TEXCOORD0;
	float2 v_wave_pos : TEXCOORD1;
};

float2 newPosition(in float2 posi, in float2 velocity){	
	float distortion = cos(radians(posi.y * 180.0 - 90.0));
	float2 offset = float2(velocity.x / distortion, -velocity.y) * 0.0001 * d_speed_factor;//changement
	float2 posf = frac(1.0 + posi + offset); 
	return posf;
}

float2 rotatePoint(in float angle, in float2 myPoint) {
	float mid = 0.5f;	
	float sint = sin(radians(angle));
	float cost = cos(radians(angle));
	float2 coord = myPoint;	
	
	coord -= 0.5;
	coord = mul(coord, float2x2(cost, -sint, sint, cost));
	coord += 0.5;
	return coord;

	//coord.x = cost * (myPoint.x - mid) + sint * (myPoint.y - mid) + mid;
	//coord.y = ((myPoint.y - mid) * cost) - ((myPoint.x - mid) * sint) + mid;

}


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
			
	//Use color for finding positions (decode particle Poition)
	output.v_particle_pos = float2(
		color.r / 255.0 + color.b,
		color.g / 255.0 + color.a
		);

	float timeFactor =  1.0f - d_lifeTime / d_maxLife;
	float mystep = step(0.5 , timeFactor);

	float valueMin = 1.0;
	float valueMax = 2.0;	
	timeFactor = 1.0 - abs(timeFactor * 2 - 1);

	float size = lerp(valueMin, valueMax, timeFactor); //LifeToReduce
	output.PointSize = size;//1.5;//Size of particle
	
	if(d_PointSpriteEnable){
		output.PointSize = 20.0;
	}

	output.positionParticle = float4(2.0 * output.v_particle_pos.x - 1.0, 1.0 - 2.0 * output.v_particle_pos.y, 0.0, 1.0); //set position in rg channels
	
	
	output.v_wave_pos = float2(0.0, 0.0); // this will get overwritten by PointSpriteEnable

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

	// I NEED TO CHANGE TEXT_coord SO
	// For rotating texture  !!!!!!!!!!!!!
	float4 color = tex2D(d_particlesSampler, IN.v_particle_pos);	

	float2 pos = float2(
		color.r / 255.0 + color.b,
		color.g / 255.0 + color.a);	
	
	float distortion = cos(radians(pos.y * 180.0 - 90.0));
	float2 offset = float2(velocity.x / distortion, -velocity.y) * 0.1 * d_speed_factor;//changement

	if (!d_PointSpriteEnable) {
		ColorRes = tex2D(d_color_rampSampler, ramp_pos);
	}else{		

		//nouvelle position
		float2 posdx = frac(1.0 + pos + offset); //frac(1.0 + pos + offset); //pos + offset = desplacement
		float4 mypos = float4(
			frac(posdx * 255.0),
			floor(posdx * 255.0) / 255.0);
		float2 delta = posdx - pos;

		float angleinRadians = atan2(pos.y - delta.y, pos.x -delta.x);
		float angle = 180 + degrees(angleinRadians);

		//if (angle < 90)
			//angleinRadians = 180;

		//if (angle < 0)
			//angle += 90;

		float2 mpoint = IN.v_wave_pos;
		float2 coord = rotatePoint(angle, mpoint);
		ColorRes = tex2D(d_waveSampler, coord);		
		//ColorRes = tex2D(d_waveSampler, IN.v_wave_pos);		
	}
	ColorRes = float4(1,0.5,0,1);
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