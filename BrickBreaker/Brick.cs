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
        public bool hasSpecialPower { get ; set; }

        public Brick(int x, int y, bool hasSpecialPower=false)
        {
            this.x = x;
            this.y = y;
            this.isDestroyed = false;
            this.hasSpecialPower = hasSpecialPower;
        }
    }

}
