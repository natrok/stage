using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D9;

namespace ConsoleApp1
{
    class Util
    {

        public static VertexBuffer createBuffer(Device device, float[] data)
        {
            VertexBuffer res = new VertexBuffer(device, data.Length * 4, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            res.Lock(0, 0, LockFlags.None).WriteRange(data);
            res.Unlock();
            return res;

        }

        public static Texture createTexture(Device device, byte[] data, int width, int height)
        {
            Texture tmp = new Texture(device, width, height, 1, Usage.None, Format.A8R8G8B8, Pool.SystemMemory);

            DataRectangle dataRectangle = tmp.LockRectangle(0, LockFlags.Discard);
            IntPtr ptr = dataRectangle.DataPointer;
            unsafe
            {
                byte* to = (byte*)ptr;
                foreach (byte value in data)
                {
                    *to++ = value;
                }
            }
            tmp.UnlockRectangle(0);

            Texture res = new Texture(device, width, height, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
            device.UpdateTexture(tmp, res);

            tmp.Dispose();
            return res;
        }

        public static Texture createTexture(Device device, byte[] data)
        {
            Texture res = Texture.FromMemory(device, data);
            return res;
        }

        public static Texture createTextureFromPath(Device device, string shaderFilePath)
        {
            Texture res = Texture.FromFile(device, shaderFilePath);
            return res;
        }

        public static Effect compileEffectProgram(Device device, string effectFilePath)
        {
            Effect res = Effect.FromFile(device, effectFilePath, ShaderFlags.UseLegacyD3DX9_31Dll);
            return res;
        }

    }

}
