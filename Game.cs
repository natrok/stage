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
        private Effect _testUpdateShader;

        //textures
        private Texture _tempTexture;
        private Texture _backGroundTexture, _screenTexture, _colorRampTexture;
        private Texture _stateParticleTexture0, _stateParticleTexture1;
        private Texture _windTexture;

        private int canvasWidth;
        private int canvasHeight;

        private float fadeOpacity = 0.97f;

        private TrackBar _slideOpacity;
        private TextBox _textBox;

        private float fadeMin = 0.96f, fadeMax = 0.999f;
        private float dropRateMin = 0.0f, dropRateMax = 0.1f;
        private float speedMin = 0.05f, speedMax = 1.0f;
        private float dropRateBumpMin = 0, dropRateBumpMax = 0.2f;
        private int numParticlesMin = 1024, numParticlesMax = 58000;


        //private ColorSlider.ColorSlider cs;

        List<Tuple<double, uint>> froidRampColors;
        private UserControl myUserControl;

        public Game()
        {

            froidRampColors = new List<Tuple<double, uint>>()
            {
            new Tuple<double, uint>(0.0, 0xffeff3ff),
            new Tuple<double, uint>(0.1, 0xffdeebf7),
            new Tuple<double, uint>(0.2, 0xffc6dbef),
            new Tuple<double, uint>(0.3, 0xff9ecae1),
            new Tuple<double, uint>(0.4, 0xff6baed6),
            new Tuple<double, uint>(0.5, 0xff4292c6),
            new Tuple<double, uint>(0.6, 0xff2171b5),
            new Tuple<double, uint>(1.0, 0xff084594)
            };

            //cs = new ColorSlider.ColorSlider(1,99,25);

            //Render form
            _RenderForm = new RenderForm("My first SharpDX app");
            _RenderForm.ClientSize = new System.Drawing.Size(_width, _height);
            _RenderForm.AllowUserResizing = false;

            myUserControl = new UserControl();
            _RenderForm.Controls.Add(myUserControl);

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
            string fileName = "2016112100";
            string PathName = "Wind";
            ReadWindData(PathName, fileName);
            InitWindTexture();

            //Initialize Objets
            InitializeRenderTexture();
            InitTexturesScreen();
            InitializeParticle();
            InitSlidesGui(myUserControl);
            InitTexturesParticles();
            InitializeShaders();

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

            Draw();
            //updateParticlesTest();
            updateParticles();
        }

        public void Draw()
        {
            updateValues();
            /*updateFadeValue();
            updateDropRate();
            updateDropRateBump();
            updateSpeed();*/

            // draw particles in _screenTexture
            RenderToSurface rts = new RenderToSurface(_Device, _width, _height, _screenTexture.GetLevelDescription(0).Format);
            Surface surface = _screenTexture.GetSurfaceLevel(0);
            rts.BeginScene(surface, new SharpDX.Viewport(0, 0, _width, _height, 0, 1));

            drawTexture(_backGroundTexture, fadeOpacity);
            drawParticles();

            rts.EndScene(Filter.None);
            surface.Dispose();
            rts.Dispose();

            //draw screenTexture in screen 
            _Device.Viewport = new SharpDX.Viewport(0, 0, canvasWidth, canvasHeight, 0, 1);
            _Device.BeginScene();
            _Device.Clear(ClearFlags.Target, new SharpDX.ColorBGRA(0.0f, 0.0f, 0.0f, 1.0f), 0.0f, 1);
            _Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            _Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            _Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

            drawTexture(_screenTexture, 1.0f);

            _Device.SetRenderState(RenderState.AlphaBlendEnable, false);
            _Device.EndScene();
            _Device.Present();

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

            //myUserControl.FadeBox.Text = "" + opacity.ToString();


            _screenShader.SetValue("d_opacity", opacity);
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
            VertexBuffer indexBuffer = _particles.getIndexBuffer();
            int numParticles = _particles.getnumParticles();

            /*set textures*/
            _drawParticleShader.SetTexture("d_wind", _windTexture);
            _Device.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
            _Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
            _drawParticleShader.SetTexture("d_particles", _stateParticleTexture0);
            _Device.SetSamplerState(1, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(1, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(1, SamplerState.MinFilter, TextureFilter.Point);
            _Device.SetSamplerState(1, SamplerState.MagFilter, TextureFilter.Point);
            _drawParticleShader.SetTexture("d_color_ramp", _colorRampTexture);
            _Device.SetSamplerState(2, SamplerState.AddressU, TextureAddress.Clamp);
            _Device.SetSamplerState(2, SamplerState.AddressV, TextureAddress.Clamp);
            _Device.SetSamplerState(2, SamplerState.MinFilter, TextureFilter.Linear);
            _Device.SetSamplerState(2, SamplerState.MagFilter, TextureFilter.Linear);

            /*Set values*/
            _drawParticleShader.SetValue("d_particles_resolution", _particles.getParticleStateResolution());
            _drawParticleShader.SetValue("d_wind_min", new float[] { _windData.uMin, _windData.vMin });
            _drawParticleShader.SetValue("d_wind_max", new float[] { _windData.uMax, _windData.vMax });

            _drawParticleShader.Begin();
            _drawParticleShader.BeginPass(0);
            _Device.VertexFormat = VertexFormat.Position;
            _Device.SetStreamSource(0, indexBuffer, 0, 4);
            _Device.DrawPrimitives(PrimitiveType.PointList, 0, numParticles);
            _drawParticleShader.EndPass();
            _drawParticleShader.End();

        }

        public void updateParticles()
        {
            int particleRes = _particles.getParticleStateResolution();

            RenderToSurface rts = new RenderToSurface(_Device, particleRes, particleRes, _stateParticleTexture1.GetLevelDescription(0).Format);
            Surface surface = _stateParticleTexture1.GetSurfaceLevel(0);
            rts.BeginScene(surface, new SharpDX.Viewport(0, 0, particleRes, particleRes, 0, 1));
            rts.Device.Clear(ClearFlags.Target, new SharpDX.ColorBGRA(1.0f, 1.0f, 1.0f, 1.0f), 0.0f, 1);

            _Device.SetRenderState(RenderState.AlphaBlendEnable, false);

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
                    _colorRampTexture = _particles.getColorRampTexture();
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
        }

        private void InitializeParticle()
        {
            _particles = new Particle(_Device);
            //_particles.setColorRamp(froidRampColors);
        }


        private void InitializeShaders()
        {
            //Compiles from file
            _drawParticleShader = Util.compileEffectProgram(_Device, "Shaders/drawParticle.fx");
            _updateParticleShader = Util.compileEffectProgram(_Device, "Shaders/updateParticle.fx");
            _screenShader = Util.compileEffectProgram(_Device, "Shaders/screenFade.fx");
            _testUpdateShader = Util.compileEffectProgram(_Device, "Shaders/testUpdate.fx");
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

        private void ReadWindData(string PathName, string fileName)
        {

            using (StreamReader r = new StreamReader(Path.Combine(PathName, fileName + ".json")))
            {
                string json = r.ReadToEnd();
                _windData = JsonConvert.DeserializeObject<WindData>(json);
            }
            _windData.image = File.ReadAllBytes(Path.Combine(PathName, fileName + ".png"));
        }
#region update with Gui
        void updateFadeValue() {
            float minimum = 0.96f;
            float maximum = 0.999f;
            float range = maximum - minimum;
            /*set values*/
            fadeOpacity = minimum + (float)(range * myUserControl.FadeBar.Value) / 100;
            myUserControl.FadeBox.Text = fadeOpacity.ToString();
        }

        void updateDropRate() {

            float dropRate = _particles.DropRate;
            dropRate = dropRateMin + (float)(dropRateMax * myUserControl.DropRateBar.Value) / myUserControl.DropRateBar.Maximum;
            _particles.DropRate = dropRate;
            myUserControl.DropRateBox.Text = _particles.DropRate.ToString();
        }

        void updateDropRateBump()
        {

            float value = _particles.DropRateBump;
            value = dropRateMin + (float)(dropRateBumpMax * myUserControl.DropRateBumpBar.Value) / myUserControl.DropRateBumpBar.Maximum;
            _particles.DropRateBump = value;
            myUserControl.DropRateBumpBox.Text = _particles.DropRateBump.ToString();
        }

        void updateSpeed()
        {
            float value = speedMin + (float)(speedMax * myUserControl.SpeedBar.Value) / myUserControl.SpeedBar.Maximum;
            _particles.SpeedFactor = value;
            myUserControl.SpeedBox.Text = _particles.SpeedFactor.ToString();
        }
        
        void updateParticlesGui()
        {
            int value = numParticlesMin + (numParticlesMax * myUserControl.NumParticlesBar.Value) / myUserControl.NumParticlesBar.Maximum;
            float fade = _particles.FadeOpacity;
            float dropRate = _particles.DropRate;
            float dropRateBump = _particles.DropRateBump;
            float speed = _particles.SpeedFactor;
            _particles = null;
            _particles = new Particle(_Device,froidRampColors,fade,speed,dropRate,dropRateBump,value);
            InitTexturesParticles();
            myUserControl.NumParticlesBox.Text = _particles.getnumParticles().ToString();
        }
#endregion
        void InitSlidesGui(UserControl myUserControl) {
            float fadeRang = fadeMax - fadeMin;
            myUserControl.FadeBar.Value = (int)(((fadeOpacity - fadeMin) * 100) / fadeRang);
            myUserControl.DropRateBar.Value = (int)(((_particles.DropRate - dropRateMin) * 1000) / (dropRateMax - dropRateMin));
            myUserControl.DropRateBumpBar.Value = (int)(((_particles.DropRate - dropRateBumpMin) * 1000) / (dropRateBumpMax - dropRateBumpMin));
            myUserControl.SpeedBar.Value = (int)(((_particles.SpeedFactor - speedMin) * 100) / (speedMax - speedMin));
            myUserControl.NumParticlesBar.Value = (int)(((_particles.getnumParticles() - numParticlesMin) * 1000) / (numParticlesMax - numParticlesMin));
            myUserControl.FadeBox.Text = fadeOpacity.ToString();
            myUserControl.DropRateBox.Text = _particles.DropRate.ToString();
            myUserControl.DropRateBumpBox.Text = _particles.DropRateBump.ToString();
            myUserControl.SpeedBox.Text = _particles.SpeedFactor.ToString();
            myUserControl.NumParticlesBox.Text = _particles.getnumParticles().ToString();
        }

        void updateValues(){
            updateFadeValue();
            updateDropRate();
            updateDropRateBump();
            updateSpeed();
            //updateParticlesGui();//trop couteux
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
            _testUpdateShader.Dispose();
            //device
            _Device.Dispose();
            _RenderForm.Dispose();
        }
    }
}
