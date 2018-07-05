
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D9;



namespace ConsoleApp1
{
    public class Particle
    {


        List<Tuple<double, uint>> defaultRampColors;
        private float fadeOpacity;
        private float speedFactor;
        private float dropRate;
        private float dropRateBump;
        private float speedReg;

        private bool isWave;

        private Texture colorRampTexture;
        private Texture particleStateTexture0;
        private Texture particleStateTexture1;


        private Device _Device;

        private int particleStateResolution;
        private int numParticles;

        
        VertexBuffer particleIndexBuffer;

        public Particle(Device _Device) {

            this._Device = _Device;

            /*Default Values*/
            defaultRampColors = new List<Tuple<double, uint>>()
            {
            new Tuple<double, uint>(0.0, 0xff3288bd),
            new Tuple<double, uint>(0.1, 0xff66c2a5),
            new Tuple<double, uint>(0.2, 0xffabdda4),
            new Tuple<double, uint>(0.3, 0xffe6f598),
            new Tuple<double, uint>(0.4, 0xfffee08b),
            new Tuple<double, uint>(0.5, 0xfffdae61),
            new Tuple<double, uint>(0.6, 0xfff46d43),
            new Tuple<double, uint>(1.0, 0xffd53e4f)
            };
            /*Default Values*/
            fadeOpacity = 0.99f; // how fast the particle trails fade on each frame
            speedFactor = 0.1f; // how fast the particles move

            dropRate = 0.006f; // how often the particles move to a random place
            dropRateBump = 0.006f; // drop rate increase relative to individual particle
            speedReg = 0.2f;

            numParticles = 6400;
            setnumParticles(numParticles);
            setColorRamp(defaultRampColors);
            isWave = false;

        }


        public Particle(Device _Device, List<Tuple<double, uint>> defaultRampColors, 
           float fadeOpacity, float speedFactor, float dropRate, float dropRateBump, int numParticles, float life, bool isWave)
        {
            this._Device = _Device;

            /*Default Values*/
            this.defaultRampColors = defaultRampColors;
            this.fadeOpacity = fadeOpacity; // how fast the particle trails fade on each frame
            this.speedFactor = speedFactor; // how fast the particles move
            this.dropRate = dropRate; // how often the particles move to a random place
            this.dropRateBump = 0.01f; // drop rate increase relative to individual particle

            this.numParticles = numParticles;
            setnumParticles(numParticles);

            setColorRamp( defaultRampColors);
            this.isWave = isWave;

        }


        public void setColorRamp(List<Tuple<double, uint>> colors)
        {
            // lookup texture for colorizing the particles according to their speed
            colorRampTexture = Util.createTexture(_Device, createColorRamp(colors), 16, 16);
        }

        public List<Tuple<double, uint>> getColorRamp()
        {

            return defaultRampColors;
            //colorRampTexture = Util.createTexture(_Device, getColorRamp(colors), 16, 16);
        }

        public Texture getColorRampTexture() {
            return colorRampTexture;
        }

        byte[] createColorRamp(List<Tuple<double, uint>> colors)
        {
            int width = 256; //16*16
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


            float[] particleIndices = new float[numParticles];
            for (int i = 0; i < numParticles; i++)
                particleIndices[i] = i;
            //set new number of particles
            this.numParticles = numParticles;
            particleIndexBuffer = Util.createBuffer(_Device, particleIndices);
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


        public VertexBuffer getIndexBuffer()
        {
            return particleIndexBuffer;
        }

        public float FadeOpacity
        {
            get { return fadeOpacity; }
            set { this.fadeOpacity = value; }
        }

        public float SpeedFactor
        {
            get { return speedFactor; }
            set { this.speedFactor = value; }
        }

        public float DropRate
        {
            get { return dropRate; }
            set { this.dropRate = value; }
        }

        public float DropRateBump
        {
            get { return dropRateBump; }
            set { this.dropRateBump = value; }
        }

        public float SpeedReg
        {
            get { return speedReg; }
            set { this.speedReg = value; }
        }


        public bool IsWave
        {
            get { return isWave; }
            set { this.isWave = value; }
        }

        /*a tester*/
        public void updateNumParticles(int numParticles) {
            setnumParticles(numParticles);
        }

    }
}
