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
    class bullet:objects
    {
        public int maxLifeTime;
        public int lifeTime;
        public bullet(Vector2 pos2, int maxLifeTime2, float ang2)
        {
            pos = pos2;
            SetSpriteCoords(26, 1);
            SetSize(4, 4);
            destroy = false;
            angle = ang2;
            AngleMath(8);
            maxLifeTime = maxLifeTime2;
        }
        public void Movment()
        {
            AngleMath(8);
            pos.X += veclocity_x;
            pos.Y += veclocity_y;
            // if outside of the screen, destroy
            if (pos.X >= 640 || pos.X <= 0 || pos.Y >= 480 || pos.Y <= 0)
            {
                destroy = true;
            }
        }
    }
}
