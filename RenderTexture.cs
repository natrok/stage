using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace ConsoleApp1
{
    class RenderTextureClass : IDisposable
    {

        /*rendering in a texture*/
        private Surface RenderSurface;
  
        private RenderToSurface RenderToSurface;
        private int height, width;
        private Device device;
        private Matrix matrix;

        public RenderTextureClass( Device device, int _width, int _height) {

            this.device = device;            
            //screenSize
            this.height = _height;
            this.width = _width;
            matrix = Matrix.Identity;
       
        }

        /*Design triangle in a texture*/
        public void drawOffTriangleInTexture(Effect program, VertexBuffer triangleVertexBuffer, Texture texture)
        {
            RenderToSurface = new RenderToSurface(device, width, height, Format.A8R8G8B8);
            RenderSurface = texture.GetSurfaceLevel(0);

            RenderToSurface.BeginScene(RenderSurface, new SharpDX.Viewport(0, 0, width, height));
            device.Clear(ClearFlags.Target, new RawColorBGRA(150, 0, 0, 140), 1.0f, 0);
            
            program.Begin();
            program.BeginPass(0); 
            
            device.VertexFormat = VertexFormat.Position | VertexFormat.Diffuse | VertexFormat.Texture1;
            device.SetStreamSource(0, triangleVertexBuffer, 0, Utilities.SizeOf<MySharpDXGame.VertexPositionColor>());

            //primitive topologie
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1); //number triangles and index init
            program.EndPass();
            program.End();
            RenderToSurface.EndScene(Filter.None);

            //Dispose
            RenderSurface.Dispose();
            RenderToSurface.Dispose();
        }

        public void drawOffParticlesInTexture(Effect program, Particle myParticle, Texture texture)
        {
            RenderToSurface = new RenderToSurface(device, width, height, Format.A8R8G8B8);
            RenderSurface = texture.GetSurfaceLevel(0);


            RenderToSurface.BeginScene(RenderSurface, new SharpDX.Viewport(0, 0, width, height));
            device.Clear(ClearFlags.Target, new RawColorBGRA(50, 0, 0, 140), 1.0f, 0);

            program.Begin();
            program.BeginPass(0);
            device.VertexFormat = VertexFormat.Position;
            device.SetStreamSource(0, myParticle.getIndexBuffer(), 0, 4);            
            device.DrawPrimitives(PrimitiveType.PointList, 0, myParticle.getnumParticles()); 
            program.EndPass();
            program.End();
            RenderToSurface.EndScene(Filter.None);

            //Dispose
            RenderSurface.Dispose();
            RenderToSurface.Dispose();
        }



        public void drawOffInTexture(Device device, Texture texture)
        {
            RenderToSurface = new RenderToSurface(device, width, height, Format.A8R8G8B8);
            RenderSurface = texture.GetSurfaceLevel(0);
            RenderToSurface.BeginScene(RenderSurface, new SharpDX.Viewport(0, 0, width, height));
            RenderToSurface.EndScene(Filter.None);
            RenderSurface.Dispose();
            RenderToSurface.Dispose();

        }

        public void setMatrix(Matrix matrix) {
            this.matrix = matrix;
        }

        public void Dispose()
        {
            RenderSurface.Dispose();
            RenderToSurface.Dispose();
        }
    }
}
