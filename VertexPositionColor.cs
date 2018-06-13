using SharpDX;
using System.Runtime.InteropServices;

namespace MySharpDXGame
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct VertexPositionColor
    {
        public readonly Vector3 Position;
        public readonly int Color;
        public readonly Vector2 TextCoord;

        public VertexPositionColor(Vector3 position, int color, Vector2 textCoord)
        {
            Position = position;
            Color = color;
            TextCoord = textCoord;
        }
    }
}