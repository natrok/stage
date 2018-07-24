
using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D9;



namespace ConsoleApp1
{
    public class Particle
    {
        private int numParticles;
        private float _Fade;
        private float _Contraste;
        private float _SpeedFactor;
        private bool _IsWave;
        private int particleStateResolution;

        private Texture colorRampTexture;
        private Texture particleStateTexture0;
        private Texture particleStateTexture1;
        List<Tuple<double, uint>> defaultGrisColors;

        private Device _Device;


        public Particle(Device _Device)
        {

            this._Device = _Device;

            /*Default Values*/
            defaultGrisColors = new List<Tuple<double, uint>>()
            {
            new Tuple<double, uint>(0.0, 0xff7f7f7f),
            new Tuple<double, uint>(0.1, 0xff7f7f7f),
            new Tuple<double, uint>(0.2, 0xff7f7f7f),
            new Tuple<double, uint>(0.3, 0xff7f7f7f),
            new Tuple<double, uint>(0.4, 0xff7f7f7f),
            new Tuple<double, uint>(0.5, 0xff7f7f7f),
            new Tuple<double, uint>(0.6, 0xff7f7f7f),
            new Tuple<double, uint>(1.0, 0xff7f7f7f)
            };
            /*Default Values*/
            _Fade = 0.98f; // how fast the particle trails fade on each frame
            _SpeedFactor = 0.5f; // how fast the particles move

            _Contraste = 0.55f;
            numParticles = 8100;
            setnumParticles(numParticles);
            setColorRamp(defaultGrisColors);
            _IsWave = false;

        }


        public Particle(Device _Device, List<Tuple<double, uint>> defaultRampColors,
           float fade, float opacity, float speedFactor, int numParticles, float life, bool isWave)
        {
            this._Device = _Device;

            /*Default Values*/
            this.defaultGrisColors = defaultRampColors;
            this._Fade = fade; // how fast the particle trails fade on each frame
            this._Contraste = opacity;// how difference contraste there is between the faster and slower particle
            this._SpeedFactor = speedFactor; // how fast the particles move
            this._IsWave = isWave;
            this.numParticles = numParticles;
            setnumParticles(numParticles);

        }


        public void setColorRamp(List<Tuple<double, uint>> colors)
        {
            // lookup texture for colorizing the particles according to their speed
            colorRampTexture = Util.createTexture(_Device, createColorRamp(colors), 16, 16);
        }


        public Texture getColorRampTexture()
        {
            return colorRampTexture;
        }

        byte[] createColorRamp(List<Tuple<double, uint>> colors)
        {
            int width = 256;
            int height = 1;

            byte[] res = new byte[width * height * 4];
            for (int i = 1; i < colors.Count; i++)
            {
                System.Drawing.Color startColor = System.Drawing.Color.FromArgb((int)colors[i - 1].Item2);
                double startCoeff = colors[i - 1].Item1;
                System.Drawing.Color endColor = System.Drawing.Color.FromArgb((int)colors[i].Item2);
                double endCoeff = colors[i].Item1;

                int startIndex = (int)(startCoeff * (width - 1));
                int endIndex = (int)(endCoeff * (width - 1));

                for (int j = startIndex; j <= endIndex; j++)
                {
                    double coeff = (j - startIndex) / (double)(endIndex - startIndex);

                    System.Drawing.Color color = System.Drawing.Color.FromArgb(
                        (byte)((1.0 - coeff) * startColor.A + coeff * endColor.A),
                        (byte)((1.0 - coeff) * startColor.R + coeff * endColor.R),
                        (byte)((1.0 - coeff) * startColor.G + coeff * endColor.G),
                        (byte)((1.0 - coeff) * startColor.B + coeff * endColor.B));

                    for (int n = 0; n < height; n++)
                    {
                        res[j * 4 + n * width * 4] = color.B;
                        res[j * 4 + 1 + n * width * 4] = color.G;
                        res[j * 4 + 2 + n * width * 4] = color.R;
                        res[j * 4 + 3 + n * width * 4] = color.A;
                    }
                }

            }

            return res;
        }



        private void setnumParticles(int numParticles)
        {
            // we create a square texture where each pixel will hold a particle position encoded as RGBA
            int particleRes = particleStateResolution = (int)Math.Ceiling(Math.Sqrt(numParticles));
            numParticles = particleRes * particleRes;

            // randomize the initial particle positions
            Random rand = new Random();
            byte[] particleState = new byte[numParticles * 4];//rgba
            for (int i = 0; i < particleState.Length; i++)
            {
                particleState[i] = (byte)Math.Floor(rand.NextDouble() * 255);
            }

            // textures to hold the particle state for the current and the next frame
            particleStateTexture0 = Util.createTexture(_Device, particleState, particleRes, particleRes);
            particleStateTexture1 = Util.createTexture(_Device, particleState, particleRes, particleRes);

            this.numParticles = numParticles;
        }

        public int getnumParticles()
        {
            return this.numParticles;
        }

        public int getParticleStateResolution()
        {
            return particleStateResolution;
        }

        public Texture getParticleStateTexture()
        {
            return particleStateTexture0;
        }

        public Texture getParticleStateTexture1()
        {
            return particleStateTexture1;
        }

        public float Fade
        {
            get { return _Fade; }
            set { _Fade = value; }
        }

        public float Contraste
        {
            get { return _Contraste; }
            set { _Contraste = value; }
        }

        public float SpeedFactor
        {
            get { return _SpeedFactor; }
            set { _SpeedFactor = value; }
        }

        public bool IsWave
        {
            get { return _IsWave; }
            set { _IsWave = value; }
        }

        public void updateNumParticles(int numParticles)
        {
            setnumParticles(numParticles);
        }

    }
}
