using MySharpDXGame;
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

namespace ConsoleApp1
{
    public class Game : IDisposable
    {
        private RenderForm _RenderForm;

        private const int _width = 1620;
        private const int _height = 1080;
        private Particle _particles;

        private WindData _windData;

        //RenderToTexture
        private RenderTextureClass _rendTexture;

        //Creates Devices
        private Device _Device;

        //shaders
        private Effect _drawParticleShader, _updateParticleShader, _screenShader;
        //private Effect _testUpdateShader;

        //textures
        private Texture _tempTexture;
        private Texture _backGroundTexture, _screenTexture;
        private Texture _stateParticleTexture0, _stateParticleTexture1;
        private Texture _windTexture, _waveTexture;
        private Texture _earthTexture, _earthNightTexture;

        private int canvasWidth;
        private int canvasHeight;
        
        private float fadeMin = 0.96f, fadeMax = 0.999f;
        private float dropRateMin = 0.0f, dropRateMax = 0.1f;
        private float speedMin = 0.05f, speedMax = 1.0f;
        private float dropRateBumpMin = 0, dropRateBumpMax = 0.2f;
        private float regSpeedMin = 0.01f, regSpeedMax = 1.0f;


        bool _wavesBool;
        private Matrix resMatrix;

      
        /*View*/
        private UserControl myUserControl;
        private ControlSettings controller;


        public Game()
        {

            //VISTA
            //Render form
            _RenderForm = new RenderForm("My first SharpDX app");
            _RenderForm.ClientSize = new System.Drawing.Size(_width, _height);
            _RenderForm.AllowUserResizing = false;

            myUserControl = new UserControl();
            _RenderForm.Controls.Add(myUserControl);
            _wavesBool = myUserControl.CheckBoxWave.Checked;

            PresentParameters p = new PresentParameters();
            p.Windowed = true;
            p.BackBufferCount = 2;
            p.BackBufferFormat = Format.X8R8G8B8;
            p.EnableAutoDepthStencil = true;
            p.AutoDepthStencilFormat = Format.D16;
            p.PresentationInterval = PresentInterval.Immediate;
            p.SwapEffect = SwapEffect.Discard;

            //Device
            _Device = new Device(new Direct3D(), 0, DeviceType.Hardware, _RenderForm.Handle, CreateFlags.HardwareVertexProcessing, p);

            //Read WindData
            string fileName = "2016112200";
            string PathName = "Wind";
            ReadWindData(PathName, fileName);
            InitWindTexture();

            //Initialize Objets

            //MODELO

            InitializeRenderTexture();
            InitTexturesScreen();
            InitializeParticle();
            InitSlidesGui(myUserControl);
            InitTexturesParticles();
            InitializeShaders();

            //CONTROL
            controller = new ControlSettings(myUserControl,_particles);
            myUserControl.setControl(controller);
            //set viewport
            resize(_width, _height);
        }

        public void Run()
        {
            RenderLoop.Run(_RenderForm, RenderCallback);
        }

        private void RenderCallback()
        {
            _Device.SetRenderState(RenderState.ZWriteEnable, false);
            _Device.SetRenderState(RenderState.StencilEnable, false);
            _Device.SetRenderState(RenderState.CullMode, Cull.None);

            updateValues();
            Draw();
            updateParticles();
        }

        public void Draw()
        {   // draw particles in _screenTexture
            RenderToSurface rts = new RenderToSurface(_Device, _width, _height, _screenTexture.GetLevelDescription(0).Format);
            Surface surface = _screenTexture.GetSurfaceLevel(0);
            rts.BeginScene(surface, new SharpDX.Viewport(0, 0, _width, _height, 0, 1));
            //Texture.ToFile(_backGroundTexture, "drawbackGround.png", ImageFileFormat.Png);
            drawTexture(_backGroundTexture, _particles.FadeOpacity);
            drawParticles();

            rts.EndScene(Filter.None);
            surface.Dispose();
            rts.Dispose();

            //draw screenTexture in screen 
            _Device.Viewport = new SharpDX.Viewport(0, 0, canvasWidth, canvasHeight, 0, 1);
            _Device.BeginScene();
            //_Device.Clear(ClearFlags.Target, new SharpDX.ColorBGRA(0.439f, 0.439f, 0.439f, 0.0f), 0.0f, 1);
            _Device.Clear(ClearFlags.Target, new SharpDX.ColorBGRA(0.0f, 0.0f, 0.0f, 1.0f), 0.0f, 1);
            _Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            _Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            _Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceColor);

            if(_wavesBool)
                drawTexture(_earthNightTexture , 1.0f);
            else
                drawTexture(_earthTexture, 1.0f);


            drawTexture(_screenTexture, 1.0f);
            _Device.SetRenderState(RenderState.AlphaBlendEnable, false);
            _Device.EndScene();
            _Device.Present();
            //Texture.ToFile(_screenTexture, "drawscreen.png", ImageFileFormat.Png);
            Texture temp = _backGroundTexture;
            _backGroundTexture = _screenTexture;
            _screenTexture = temp;

        }

        /*Shader*/
        void drawTexture(Texture texture, float opacity)
        {
            _screenShader.SetTexture("d_screen", texture);
            _Device.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Point);
            _Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Point);

            _screenShader.SetValue("d_opacity", _particles.FadeOpacity);
            //_screenShader.SetValue("d_opacity", fadeOpacity);
            // patch pour directx 9, ne pas avoir se code en open gl es
            _screenShader.SetValue("d_miPixX", -0.5f / texture.GetLevelDescription(0).Width);
            _screenShader.SetValue("d_miPixY", -0.5f / texture.GetLevelDescription(0).Height);

            _screenShader.Begin();
            _screenShader.BeginPass(0);
            UtilDraw.drawInQuad(_Device);
            _screenShader.EndPass();
            _screenShader.End();
        }


        public void drawParticles()
        {

            _Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            _Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            _Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);


            VertexBuffer indexBuffer = _particles.getIndexBuffer();
            int numParticles = _particles.getnumParticles();
            _Device.SetRenderState(RenderState.PointSpriteEnable, _wavesBool);
            /*set textures*/
            _drawParticleShader.SetTexture("d_wind", _windTexture);
            _Device.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
            _Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
            _drawParticleShader.SetTexture("d_wave", _waveTexture);
            _Device.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
            _Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
            _drawParticleShader.SetTexture("d_particles", _stateParticleTexture0);
            _Device.SetSamplerState(1, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(1, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(1, SamplerState.MinFilter, TextureFilter.Point);
            _Device.SetSamplerState(1, SamplerState.MagFilter, TextureFilter.Point);
            _drawParticleShader.SetTexture("d_color_ramp", _particles.getColorRampTexture());
            _Device.SetSamplerState(2, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(2, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(2, SamplerState.MinFilter, TextureFilter.Linear);
            _Device.SetSamplerState(2, SamplerState.MagFilter, TextureFilter.Linear);
           //localMatrix = Matrix.Identity;

            /*Set values*/
            _drawParticleShader.SetValue("d_particles_resolution", _particles.getParticleStateResolution());
            _drawParticleShader.SetValue("d_wind_min", new float[] { _windData.uMin, _windData.vMin });
            _drawParticleShader.SetValue("d_wind_max", new float[] { _windData.uMax, _windData.vMax });
            _drawParticleShader.SetValue("d_speed_factor", _particles.SpeedFactor);
            //_wavesBool = true;
            //_drawParticleShader.SetValue("d_PointSpriteEnable", _wavesBool);
            _drawParticleShader.SetValue("d_PointSpriteEnable", false);
            //_drawParticleShader.SetValue("rotation", -1.0f * (float)Math.PI/2 );


            _drawParticleShader.Begin();
            _drawParticleShader.BeginPass(0);
            _Device.VertexFormat = VertexFormat.Position;
            _Device.SetStreamSource(0, indexBuffer, 0, 4);
            _Device.DrawPrimitives(PrimitiveType.PointList, 0, numParticles);
            //_Device.DrawPrimitives(PrimitiveType.LineList, 0, numParticles);
            _drawParticleShader.EndPass();
            _drawParticleShader.End();

        }

        public void updateParticles()
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

            _updateParticleShader.SetValue("d_speed_factor", _particles.SpeedFactor);
            _updateParticleShader.SetValue("d_drop_rate", _particles.DropRate);
            _updateParticleShader.SetValue("d_drop_rate_bump", _particles.DropRateBump);
            _updateParticleShader.SetValue("d_speed_reg", _particles.SpeedReg);

            _updateParticleShader.SetValue("d_voisin", true);




            _updateParticleShader.Begin();
            _updateParticleShader.BeginPass(0);
            UtilDraw.drawInQuad(_Device);
            _updateParticleShader.EndPass();
            _updateParticleShader.End();

            rts.EndScene(Filter.None);
            surface.Dispose();
            rts.Dispose();
            //Texture.ToFile(_stateParticleTexture1, "_stateParticleTextureAfter.png", ImageFileFormat.Png);

            /*exchange texture*/
            _tempTexture = _stateParticleTexture0;
            _stateParticleTexture0 = _stateParticleTexture1;
            _stateParticleTexture1 = _tempTexture;
        }
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

            _waveTexture = Texture.FromFile(_Device, "Img/wave2.png");

            try
            {
                if (!_particles.Equals(null))
                {
                    _stateParticleTexture0 = _particles.getParticleStateTexture();
                    _stateParticleTexture1 = _particles.getParticleStateTexture1(); ;//initially are the same
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

        }


        private void InitializeRenderTexture()
        {
            _rendTexture = new RenderTextureClass(_Device, _width, _height);
            _earthTexture = Texture.FromFile(_Device, "Img/earth.jpg");
            _earthNightTexture = Texture.FromFile(_Device, "Img/earth_night.jpg");
        }

        private void InitializeParticle()
        {
            _particles = new Particle(_Device);
        }


        private void InitializeShaders()
        {
            //Compiles from file
            _drawParticleShader = Util.compileEffectProgram(_Device, "Shaders/drawParticle.fx");
            _updateParticleShader = Util.compileEffectProgram(_Device, "Shaders/updateParticle.fx");
            _screenShader = Util.compileEffectProgram(_Device, "Shaders/screenFade.fx");
        }
        public void resize(int width, int height)
        {
            canvasWidth = width;
            canvasHeight = height;
            //const gl = this.gl;
            byte[] emptyPixels = new byte[width * height * 4];
            // screen textures to hold the drawn screen for the previous and the current frame
            _backGroundTexture = Util.createTexture(_Device, emptyPixels, width, height);
            _screenTexture = Util.createTexture(_Device, emptyPixels, width, height);


        }

        void InitSlidesGui(UserControl myUserControl)
        {
            myUserControl.FadeBar.Value = (int)(((_particles.FadeOpacity - fadeMin) * 100) / (fadeMax - fadeMin));
            myUserControl.DropRateBar.Value = (int)(((_particles.DropRate - dropRateMin) * 1000) / (dropRateMax - dropRateMin));
            myUserControl.DropRateBumpBar.Value = (int)(((_particles.DropRate - dropRateBumpMin) * 1000) / (dropRateBumpMax - dropRateBumpMin));
            myUserControl.SpeedBar.Value = (int)(((_particles.SpeedFactor - speedMin) * 100) / (speedMax - speedMin));
            myUserControl.RegSpeedBar.Value = (int)(((_particles.SpeedReg - regSpeedMin) * 100) / (regSpeedMax - regSpeedMin));
            myUserControl.FadeBox.Text = _particles.FadeOpacity.ToString();
            myUserControl.DropRateBox.Text = _particles.DropRate.ToString();
            myUserControl.DropRateBumpBox.Text = _particles.DropRateBump.ToString();
            myUserControl.SpeedBox.Text = _particles.SpeedFactor.ToString();
            myUserControl.RegSpeedBox.Text = _particles.SpeedReg.ToString();

        }
#endregion
        private void ReadWindData(string PathName, string fileName)
        {

            using (StreamReader r = new StreamReader(Path.Combine(PathName, fileName + ".json")))
            {
                string json = r.ReadToEnd();
                _windData = JsonConvert.DeserializeObject<WindData>(json);
            }
            _windData.image = File.ReadAllBytes(Path.Combine(PathName, fileName + ".png"));
        }

        void updateValues(){
            _wavesBool = myUserControl.CheckBoxWave.Checked;
        }


        public void Dispose()
        {
            //textures
            _windTexture.Dispose();
            _stateParticleTexture1.Dispose();
            _stateParticleTexture0.Dispose();
            _screenTexture.Dispose();
            _backGroundTexture.Dispose();
            //shader
            _drawParticleShader.Dispose();
            _updateParticleShader.Dispose();

            //device
            _Device.Dispose();
            _RenderForm.Dispose();
        }
    }
}
