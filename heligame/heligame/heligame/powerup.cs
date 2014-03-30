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
    class powerup:objects
    {
        public int type;
        public int lifeTime;

        public powerup(Vector2 pos2, int type2)
        {
            destroy = false;
            pos = pos2;
            type = type2;
            SetSize(24, 24);
            // So that the power ups have diffrent types
            switch (type)
            {
                case 1:
                    SetSpriteCoords(1, Frame(8));
                    break;
                case 2:
                    SetSpriteCoords(1, Frame(9));
                    break;
            }
        }
        public void Update(player player, SoundEffect powerUpSfx)
        {
            // Destroy the power up so that their is never two power ups at the same time
            lifeTime += 1;
            if (lifeTime >= 128*2)
            {
                destroy = true;
            }
            Random random = new Random();
            Rectangle playerC = new Rectangle((int)player.pos.X - 12, (int)player.pos.Y - 12, 24, 24);
            Rectangle powerUpC = new Rectangle((int)pos.X - 12, (int)pos.Y - 12, 24, 24);
            if (playerC.Intersects(powerUpC))
            {
                // the player can't get more than 3 hp
                if (type == 1 && player.hp < 3)
                {
                    powerUpSfx.Play();
                    player.hp += 1;
                    destroy = true;
                }
                // the player can only have one gun at the time
                if (type == 2 && player.gunType == 1)
                {
                    powerUpSfx.Play();
                    player.gunType = random.Next(2,4);
                    destroy = true;
                }
            }
        }
    }
}
