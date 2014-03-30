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
    class enemyBullet:objects
    {
        public enemyBullet(Vector2 pos2, Vector2 posAim, float ang2)
        {
            Random random = new Random();
            SetSpriteCoords(26, 5);
            SetSize(4, 4);
            pos = pos2;
            MathAim(7, posAim.X + random.Next(-25, 25), posAim.Y + random.Next(-25, 25));
        }
        public void Movment()
        {
            pos.X += veclocity_x;
            pos.Y += veclocity_y;
            if (pos.X >= 640 || pos.X <= 0 || pos.Y >= 480 || pos.Y <= 0)
            {
                destroy = true;
            }
        }
        public void Update(player player)
        {
            Rectangle enemyBulletC = new Rectangle((int)pos.X-2, (int)pos.Y-2, 4, 4);
            Rectangle playerC = new Rectangle((int)player.pos.X - 12, (int)player.pos.Y - 12, 24, 24);
            if (enemyBulletC.Intersects(playerC))
            {
                player.hp -= 1;
                player.hit = true;
                destroy = true;
            }
        }
    }
}
