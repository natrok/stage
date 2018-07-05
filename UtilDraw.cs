using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class UtilDraw
    {
        public static void drawTextureFromSprite(Device device, Texture texture)
        {
            using (Sprite spriteobject = new Sprite(device))
            {
                spriteobject.Begin(SpriteFlags.DoNotSaveState);
                spriteobject.Draw(texture, SharpDX.Color.White);
                spriteobject.End();
                spriteobject.Dispose();
            }
        }

        public static void drawInQuad(Device device)
        {
            
            VertexBuffer quadBuffer = Util.createBuffer(device, new float[] { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0 }); 
            device.VertexFormat = VertexFormat.Position;
            device.SetStreamSource(0, quadBuffer, 0, 12);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            
            /*
            VertexBuffer quadBuffer = Util.createBuffer(device, new float[] { -1, -1, 1, -1, 1, 1, -1, 1 });
            device.VertexFormat = VertexFormat.Position;
            device.SetStreamSource(0, quadBuffer, 0, 8);
            device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 4);
            */
        }

        public static void drawBackgroundTextureFromSprite(Device device, Texture offscreen1, Texture offscreen2)
        {
            using (Sprite spriteobject = new Sprite(device))
            {
                spriteobject.Begin(SpriteFlags.AlphaBlend);
                spriteobject.Draw(offscreen1, SharpDX.Color.White);
                spriteobject.Draw(offscreen2, SharpDX.Color.White);

                spriteobject.End();
                spriteobject.Dispose();
            }
            
        }

        public static void drawTextureFromSprite(Device device, Texture texture,  int widht, int height)
        {
            using (Sprite spriteobject = new Sprite(device))
            {
                spriteobject.Begin(SpriteFlags.DoNotSaveState);
                spriteobject.Draw(texture, SharpDX.Color.White, new SharpDX.Rectangle(0, 0, widht, height), new Vector3(0, 0, 0), new Vector3(0, 0, 0f));
                spriteobject.End();
            }
        }

    }
}
