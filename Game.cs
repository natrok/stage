//using MySharpDXGame;
using Newtonsoft.Json;

//Sharpdx
using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using SharpDX.Direct3D9;

using System;
using System.IO;
using System.Collections.Generic;

using System.Windows.Forms;
using System.Diagnostics;

namespace ConsoleApp1
{
	public class Game : IDisposable
	{
		private RenderForm _RenderForm;

		private int _width = Screen.PrimaryScreen.Bounds.Width;// 1620;
		private int _height = Screen.PrimaryScreen.Bounds.Height;  //1080;
		private int dx = 0, min_range = 0, max_range;

		//Fade transition of particle
		private float[] f = { 0.3f, 0.5f, 0.8f, 1.0f, 1.0f, 0.8f, 0.5f, 0.1f };
		private float[] fadeTableau = { 0.1f, 0.3f, 0.6f, 1.0f, 1.0f, 1.0f, 1.0f, 0.70f, 0.4f, 0.1f };



		private Particle _particles;
		private WindData _windData;

		//Creates Devices
		private Device _Device;

		//shaders
		private Effect _instanceShader, _updateParticleShader, _screenShader;


		//textures
		private Texture _tempTexture;
		private Texture _backGroundTexture, _screenTexture;
		private Texture _stateParticleTexture0, _stateParticleTexture1;

		private Texture _windTexture;
		private Texture _earthTexture, _earthWhite, _earthNightTexture, _maxSeaTexture;

		private int canvasWidth;
		private int canvasHeight;


		private IndexBuffer _IndexBuffer;
		private VertexBuffer _quadVertexBuffer;

		bool _pointBool;


		//Changing ***********
		int vertexTotal;
		int _particlesPerBlock;
		int numberOfBlocks;



		/*View*/
		private UserView myUserView;
		private ControlSettings controller;

		private Stopwatch ft = new Stopwatch();

		public Game()
		{
			//View
			//Render form
			_RenderForm = new RenderForm("My first SharpDX app");
			_RenderForm.ClientSize = new System.Drawing.Size(_width, _height);
			_RenderForm.AllowUserResizing = false;

			myUserView = new UserView();
			_RenderForm.Controls.Add(myUserView);

			PresentParameters p = new PresentParameters();
			p.Windowed = true;
			p.BackBufferCount = 2;
			p.BackBufferFormat = Format.X8R8G8B8;
			p.EnableAutoDepthStencil = true;
			p.AutoDepthStencilFormat = Format.D16;
			p.PresentationInterval = PresentInterval.Immediate;
			p.SwapEffect = SwapEffect.Discard;

			//******************
			numberOfBlocks = fadeTableau.Length;



			//Device
			_Device = new Device(new Direct3D(), 0, DeviceType.Hardware, _RenderForm.Handle, CreateFlags.HardwareVertexProcessing, p);

			//Read WindData
			string fileName = "2016112100";
			string PathName = "Wind";
			ReadWindData(PathName, fileName);
			InitWindTexture();

			//Initialize Objets
			//Model
			InitializeRenderTexture();
			InitTexturesScreen();
			InitializeParticle();
			InitTexturesParticles();
			InitializeShaders();

			CreateVBIB();

			//CONTROL
			controller = new ControlSettings(myUserView, _particles);
			controller.InitSlidesGui();

			myUserView.setControl(controller);
			//set viewport
			Resize(_width, _height);
		}

		public void Run()
		{
			ft.Start();
			RenderLoop.Run(_RenderForm, RenderCallback);
			ft.Stop();

		}

		private void RenderCallback()
		{
			_Device.SetRenderState(RenderState.ZWriteEnable, false);
			_Device.SetRenderState(RenderState.StencilEnable, false);
			_Device.SetRenderState(RenderState.CullMode, Cull.None);

			if (controller.IsChangedParticle)
			{
				_stateParticleTexture0 = _particles.getParticleStateTexture();
				_stateParticleTexture1 = _particles.getParticleStateTexture1();
				CreateVBIB();
				_particlesPerBlock = (int)Math.Round((float)(_particles.getnumParticles() / numberOfBlocks)); //****
				controller.IsChangedParticle = false;
			}

			updateValues();

			Draw();
			updateParticles();
		}

		public void Draw()
		{   // draw particles in _screenTexture
			RenderToSurface rts = new RenderToSurface(_Device, _width, _height, _screenTexture.GetLevelDescription(0).Format);
			Surface surface = _screenTexture.GetSurfaceLevel(0);
			rts.BeginScene(surface, new SharpDX.Viewport(0, 0, _width, _height, 0, 1));
			drawTexture(_backGroundTexture, _particles.Fade);
			DrawQuadParticles();
			rts.EndScene(Filter.None);
			surface.Dispose();
			rts.Dispose();

			//draw screenTexture in screen 
			_Device.Viewport = new SharpDX.Viewport(0, 0, canvasWidth, canvasHeight, 0, 1);
			_Device.BeginScene();
			//
			_Device.Clear(ClearFlags.Target, new SharpDX.ColorBGRA(0.0f, 0.0f, 0.0f, 1.0f), 0.0f, 1);
			_Device.SetRenderState(RenderState.AlphaBlendEnable, true);
			_Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha); //SourceColor SourceAlpha SourceAlpha*
			_Device.SetRenderState(RenderState.DestinationBlend, Blend.DestinationAlpha); //DestinationAlpha SourceColor  DestinationColor*																						  

			if (_pointBool)
				drawTexture(_earthTexture, 1.0f);
			else
				drawTexture(_earthWhite, 1.0f);

			drawTexture(_screenTexture, 1.0f);
			_Device.SetRenderState(RenderState.AlphaBlendEnable, false);
			_Device.EndScene();
			_Device.Present();

			Texture temp = _backGroundTexture;
			_backGroundTexture = _screenTexture;
			_screenTexture = temp;
		}

		#region Draw Particles
		/*Shader*/
		void drawTexture(Texture texture, float opacity)
		{
			_screenShader.SetTexture("d_screen", texture);
			_Device.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
			_Device.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);
			_Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Point);
			_Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Point);

			_screenShader.SetValue("d_opacity", _particles.Fade);
			// patch pour directx 9, ne pas avoir se code en open gl es
			_screenShader.SetValue("d_miPixX", -0.5f / texture.GetLevelDescription(0).Width);
			_screenShader.SetValue("d_miPixY", -0.5f / texture.GetLevelDescription(0).Height);
			_screenShader.Begin();
			_screenShader.BeginPass(0);
			UtilDraw.drawInQuad(_Device);
			_screenShader.EndPass();
			_screenShader.End();
		}

		private void updateIndexReset()
		{
			float time = ft.ElapsedMilliseconds;
			if (time > 200f)
			{
				ft.Restart();
				dx = (dx + 1) % numberOfBlocks;

				//the direction of min and max range could to change
				min_range = dx * _particlesPerBlock;
				max_range = min_range + _particlesPerBlock;
			}
			else
			{
				min_range = 0;
				max_range = 0;
			}
		}

		private void CreateVBIB()
		{
			int numParticles = _particles.getnumParticles(); // => particleRes * particleRes
			int particleRes = _particles.getParticleStateResolution();

			//Fill indices
			int indsPerParticle = 6;
			int vertsPerParticle = 4;
			int[] indices = new int[numParticles * indsPerParticle]; // 6 indices for 2 triangles = 1 quad
			int n = 0;
			for (int i = 0; i < numParticles; i++)
			{
				indices[i * indsPerParticle] = n;
				indices[i * indsPerParticle + 1] = n + 1;
				indices[i * indsPerParticle + 2] = n + 2;
				indices[i * indsPerParticle + 3] = n + 3;
				indices[i * indsPerParticle + 4] = indices[i * indsPerParticle];
				indices[i * indsPerParticle + 5] = indices[i * indsPerParticle + 2];
				n += vertsPerParticle;
			}

			//Create Index Buffer
			_IndexBuffer = new IndexBuffer(_Device, indices.Length * 4, Usage.WriteOnly, Pool.Managed, false);
			_IndexBuffer.Lock(0, 0, LockFlags.None).WriteRange(indices);//Write array of values
			_IndexBuffer.Unlock();
			_Device.Indices = _IndexBuffer;

			//Remplir Vertex
			vertexTotal = particleRes * particleRes * vertsPerParticle;
			Vector4[] _myVerticesColorQuad = new Vector4[vertexTotal];

			//Flag vertex par particle
			int[] m = { 0, 0, 0, 1, 1, 1, 1, 0 };
			int value = 0;
			for (int i = 0; i < numParticles; i++)
			{
				//flags of quad
				for (int r = 0; r < vertsPerParticle; r++)
				{
					_myVerticesColorQuad[value] = new Vector4(i, 0, m[2 * r], m[2 * r + 1]);
					value++;
				}
			}

			//Create Vertex Buffer
			_quadVertexBuffer = new VertexBuffer(_Device, Utilities.SizeOf<Vector4>() * _myVerticesColorQuad.Length, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
			_quadVertexBuffer.Lock(0, 0, LockFlags.None).WriteRange(_myVerticesColorQuad);
			_quadVertexBuffer.Unlock();
		}

		private void DrawQuadParticles()
		{
			_Device.Viewport = new SharpDX.Viewport(0, 0, canvasWidth, canvasHeight, 0, 1);
			_Device.SetRenderState(RenderState.ZWriteEnable, false);
			_Device.SetRenderState(RenderState.StencilEnable, false);
			_Device.SetRenderState(RenderState.CullMode, Cull.None);

			_Device.VertexFormat = VertexFormat.PositionW;
			_Device.SetStreamSource(0, _quadVertexBuffer, 0, Utilities.SizeOf<Vector4>());

			_instanceShader.Begin();
			_instanceShader.BeginPass(0);

			//Set shaders Values. 
			_instanceShader.SetTexture("d_wind", _windTexture);
			_Device.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
			_Device.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);
			_Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
			_Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
			_instanceShader.SetTexture("d_State0", _stateParticleTexture0);
			_Device.SetSamplerState(1, SamplerState.AddressU, TextureAddress.Clamp);
			_Device.SetSamplerState(1, SamplerState.AddressV, TextureAddress.Clamp);
			_Device.SetSamplerState(1, SamplerState.MinFilter, TextureFilter.Point);
			_Device.SetSamplerState(1, SamplerState.MagFilter, TextureFilter.Point);
			_instanceShader.SetTexture("d_State1", _stateParticleTexture1);
			_Device.SetSamplerState(2, SamplerState.AddressU, TextureAddress.Clamp);
			_Device.SetSamplerState(2, SamplerState.AddressV, TextureAddress.Clamp);
			_Device.SetSamplerState(2, SamplerState.MinFilter, TextureFilter.Point);
			_Device.SetSamplerState(2, SamplerState.MagFilter, TextureFilter.Point);
			_instanceShader.SetTexture("d_color_ramp", _particles.getColorRampTexture());
			_Device.SetSamplerState(3, SamplerState.AddressU, TextureAddress.Clamp);
			_Device.SetSamplerState(3, SamplerState.AddressV, TextureAddress.Clamp);
			_Device.SetSamplerState(3, SamplerState.MinFilter, TextureFilter.Linear);
			_Device.SetSamplerState(3, SamplerState.MagFilter, TextureFilter.Linear);

			_instanceShader.SetValue("d_particles_res", _particles.getParticleStateResolution());
			_instanceShader.SetValue("d_resScreen", new float[] { _width, _height });
			_instanceShader.SetValue("d_wind_min", new float[] { _windData.uMin, _windData.vMin });
			_instanceShader.SetValue("d_wind_max", new float[] { _windData.uMax, _windData.vMax });
			_instanceShader.SetValue("d_contraste", _particles.Contraste);
			_instanceShader.SetValue("d_waves", _particles.IsWave);

			/************************/
			_instanceShader.SetValue("d_ParticulesByBlock", _particlesPerBlock);
			_instanceShader.SetValue("d_fadeTableau", fadeTableau);
			_instanceShader.SetValue("d_delta", dx);
			/************************/

			_Device.DrawIndexedPrimitive(PrimitiveType.TriangleList, 0, 0, vertexTotal, 0, 2 * _particles.getnumParticles());
			_instanceShader.EndPass();
			_instanceShader.End();

		}

		private void updateParticles()
		{
			int particleRes = _particles.getParticleStateResolution();
			RenderToSurface rts = new RenderToSurface(_Device, particleRes, particleRes, _stateParticleTexture1.GetLevelDescription(0).Format);
			Surface surface = _stateParticleTexture1.GetSurfaceLevel(0);
			rts.BeginScene(surface, new SharpDX.Viewport(0, 0, particleRes, particleRes, 0, 1));

			/*set parametres shader*/
			_updateParticleShader.SetTexture("d_wind", _windTexture);
			_Device.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
			_Device.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);
			_Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
			_Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);

			_updateParticleShader.SetTexture("d_particles", _stateParticleTexture0);
			_Device.SetSamplerState(1, SamplerState.AddressU, TextureAddress.Clamp);
			_Device.SetSamplerState(1, SamplerState.AddressV, TextureAddress.Clamp);
			_Device.SetSamplerState(1, SamplerState.MinFilter, TextureFilter.Point);
			_Device.SetSamplerState(1, SamplerState.MagFilter, TextureFilter.Point);

			//parametros JSON
			_updateParticleShader.SetValue("d_rand_seed", (float)new Random().NextDouble());
			_updateParticleShader.SetValue("d_wind_resolution", new float[] { _windData.width, _windData.height });
			_updateParticleShader.SetValue("d_wind_min", new float[] { _windData.uMin, _windData.vMin });
			_updateParticleShader.SetValue("d_wind_max", new float[] { _windData.uMax, _windData.vMax });
			_updateParticleShader.SetValue("d_particles_res", particleRes);
			_updateParticleShader.SetValue("d_particles_min", min_range);
			_updateParticleShader.SetValue("d_particles_max", max_range);

			//parametros Menu
			_updateParticleShader.SetValue("d_speed_factor", _particles.SpeedFactor);

			//correction texture inverse
			_updateParticleShader.SetValue("d_miPixX", -0.5f / _stateParticleTexture0.GetLevelDescription(0).Width);
			_updateParticleShader.SetValue("d_miPixY", -0.5f / _stateParticleTexture0.GetLevelDescription(0).Height);

			if (_particles.IsWave)
				_updateParticleShader.Technique = "Wave";
			else
				_updateParticleShader.Technique = "Default";

			/************************/
			_updateParticleShader.SetValue("d_ParticulesByBlock", _particlesPerBlock);
			_updateParticleShader.SetValue("d_delta", dx);
			/************************/

			_updateParticleShader.Begin();
			_updateParticleShader.BeginPass(0);
			UtilDraw.drawInQuad(_Device);
			_updateParticleShader.EndPass();
			_updateParticleShader.End();

			rts.EndScene(Filter.None);
			surface.Dispose();
			rts.Dispose();

			/*Change index for fading and reset*/
			updateIndexReset();

			/*exchange texture*/
			_tempTexture = _stateParticleTexture0;
			_stateParticleTexture0 = _stateParticleTexture1;
			_stateParticleTexture1 = _tempTexture;

		}
		#endregion

		#region initialize Variables
		private void InitTexturesScreen()
		{
			byte[] emptyPixels = new byte[_width * _height * 4]; //RGBA
			_backGroundTexture = Util.createTexture(_Device, emptyPixels, _width, _height);
			_screenTexture = Util.createTexture(_Device, emptyPixels, _width, _height);
			_tempTexture = Util.createTexture(_Device, emptyPixels, _width, _height);
		}

		private void InitWindTexture()
		{
			if (!_windData.Equals(null))
			{
				_windTexture = Util.createTexture(_Device, _windData.image);
			}
		}

		private void InitTexturesParticles()
		{
			try
			{
				if (!_particles.Equals(null))
				{
					_stateParticleTexture0 = _particles.getParticleStateTexture();
					_stateParticleTexture1 = _particles.getParticleStateTexture1();
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.ToString());
			}
		}


		private void InitializeRenderTexture()
		{

			_earthTexture = Texture.FromFile(_Device, "Img/earth.jpg");
			_earthWhite = Texture.FromFile(_Device, "Img/earth_white.jpg");
			_earthNightTexture = Texture.FromFile(_Device, "Img/earth_night.jpg");
			_maxSeaTexture = Texture.FromFile(_Device, "Img/franceMaxSea.png");
		}

		private void InitializeParticle()
		{
			_particles = new Particle(_Device);
			_particlesPerBlock = (int)Math.Round((float)(_particles.getnumParticles() / numberOfBlocks));
		}

		private void InitializeShaders()
		{

			_updateParticleShader = Util.compileEffectProgram(_Device, "Shaders/updateParticle.fx");
			_screenShader = Util.compileEffectProgram(_Device, "Shaders/screenFade.fx");
			//shader replace drawParticle            
			_instanceShader = Util.compileEffectProgram(_Device, "Shaders/drawQuadParticle.fx");

		}
		public void Resize(int width, int height)
		{
			canvasWidth = width;
			canvasHeight = height;
			byte[] emptyPixels = new byte[width * height * 4];
			// screen textures to hold the drawn screen for the previous and the current frame
			_backGroundTexture = Util.createTexture(_Device, emptyPixels, width, height);
			_screenTexture = Util.createTexture(_Device, emptyPixels, width, height);
		}


		private void ReadWindData(string PathName, string fileName)
		{

			using (StreamReader r = new StreamReader(Path.Combine(PathName, fileName + ".json")))
			{
				string json = r.ReadToEnd();
				_windData = JsonConvert.DeserializeObject<WindData>(json);
			}
			_windData.image = File.ReadAllBytes(Path.Combine(PathName, fileName + ".png"));
		}
		#endregion
		void updateValues()
		{
			_pointBool = myUserView.CheckBoxPoint.Checked;
		}


		public void Dispose()
		{
			//textures
			_windTexture.Dispose();
			_stateParticleTexture1.Dispose();
			_stateParticleTexture0.Dispose();
			_screenTexture.Dispose();
			_backGroundTexture.Dispose();
			_earthWhite.Dispose();
			_earthTexture.Dispose();

			//VBIB
			_quadVertexBuffer.Dispose();
			_IndexBuffer.Dispose();

			//shader
			_updateParticleShader.Dispose();
			_screenShader.Dispose();
			_instanceShader.Dispose();

			//device
			_Device.Dispose();
			_RenderForm.Dispose();
		}
	}
}
