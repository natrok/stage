using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D9;
/*private D3D9.VertexShader _VertexShader;
private D3D9.PixelShader _FragmentShader;*/
namespace ConsoleApp1
{
    class ShaderUtil
    {

        private const string defaultName = "default.fx";

        public static Effect ShaderUtilInit(Device device, string shaderNamePath) {
            return Effect.FromFile(device, shaderNamePath, ShaderFlags.UseLegacyD3DX9_31Dll);
        }

        public static void setTextureProgram(Effect program, Texture myTexture, string varTxtName)
        {
            program.SetTexture(varTxtName, myTexture);
        }

        public static void setValueProgram(Effect program, Matrix myMatrix, string varTxtName)
        {
            program.SetValue(varTxtName, myMatrix);
        }

    }
}
