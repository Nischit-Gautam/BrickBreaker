using System;
using System.Collections.Generic;
using System.Text;

namespace BrickBreaker
{
    public class Brick
    {
        public int x { get; set; } // x position of brick
        public int y { get; set; } // y position of brick
        public bool isDestroyed { get; set; } // flag to check if the brick is destroyed

        public Brick(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.isDestroyed = false;
        }
    }

}
