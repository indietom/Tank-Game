using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace heligame
{
    class particle:objects
    {
        public int type;

        public particle(Vector2 pos2, float spe, int type2, float ang2, string color)
        {
            Random random = new Random();
            pos = pos2;
            type = type2;
            angle = ang2;
            SetSize(2, 2);
            switch (color)
            {
                case "red":
                    SetSpriteCoords(Frame(1), 7);
                    break;
                case "grey":
                    SetSpriteCoords(Frame(1), 23);
                    break;
                case "orange":
                    SetSpriteCoords(Frame(1), 10);
                    break;
            }
            switch (type)
            {
                case 1:
                    accel = spe;
                    break;
                case 2:
                    accel = spe;
                    break;
            }
        }

        public void Movment()
        {
            switch (type)
            {
                case 1:
                    // slow down the particles so that it looks likes blood/wreckege
                    AngleMath(accel);
                    if (accel > 0)
                    {
                        accel -= 0.5f;
                    }
                    pos.X += veclocity_x;
                    pos.Y += veclocity_y;
                    break;
                case 2:
                    // slow down and destroy so that it looks like an explosion
                    AngleMath(accel);
                    accel -= 0.5f;
                    if (accel <= 0)
                    {
                        destroy = true;
                    }
                    pos.X -= veclocity_x;
                    pos.Y -= veclocity_y;
                    break;
            }
            // it's a waste of memoery to let the particles outside of the screen
            if (pos.X >= 640 || pos.X <= 0 || pos.Y >= 480 || pos.Y <= 0)
            {
                destroy = true;
            }
        }
    }
}
