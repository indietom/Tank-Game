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
    class explosion:objects
    {
        public int currentFrame;

        public explosion(Vector2 pos2)
        {
            pos = pos2;
            SetSpriteCoords(1, 151);
            SetSize(24, 24);
            currentFrame = 1;
        }

        public void Animation(List<particle> particles)
        {
            Random random = new Random();
            animationCount += 1;
            if (animationCount >= 5)
            {
                currentFrame += 1;
                if (currentFrame == 1)
                {
                    SetSpriteCoords(1, 151);
                }
                else
                {
                    SetSpriteCoords(Frame(currentFrame), 151);
                }
                if (currentFrame == 2)
                {
                    for (int i = 0; i < 20; i++)
                        particles.Add(new particle(pos, random.Next(5, 15), 2, random.Next(360), "orange"));
                }
                if (currentFrame >= 4)
                {
                    // if the explosion's animation is done  it's removed
                    destroy = true;
                }
                animationCount = 0;
            }
        }
    }
}
