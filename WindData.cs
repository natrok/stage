using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class WindData
    {
        public string source;
        public DateTime date;
        public int width;
        public int height;
        public float uMin;
        public float uMax;
        public float vMin;
        public float vMax;

        public byte[] image;
    }
}
