using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapGenerator
{
    internal class Chunk
    {
        private int _topLeftX;
        private int _topLeftY;
        private int _size;
        public int Size { get { return _size; } }

        public Chunk(int seed, int tlX, int tlY) 
        {
            _topLeftX = tlX;
            _topLeftY = tlY;
        }
        //public void Generate(int seed, )
        //{

        //}
    }
}
