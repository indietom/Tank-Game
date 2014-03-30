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
    class objects
    {
        public Vector2 pos;

        public int imx;
        public int imy;
        public int width;
        public int height;
        public int hp;
        // keeps count on when to switch frame
        public int animationCount;

        public bool destroy;

        public float maxAccel;
        public float minAccel;
        public float accel;
        public float angle2;
        public float angle;
        public float speed;
        public float scale_x;
        public float scale_y;
        public float veclocity_x;
        public float veclocity_y;

        public int Frame(int frame2)
        {
            if (frame2 == -1)
            {
                return 1;
            }
            return 24 * frame2 + frame2 + 1;
        }

        // returns the distance between x, y and x2, y2
        public float DistanceTo(float x2, float y2)
        {
            return (float)Math.Sqrt((pos.X - x2) * (pos.X - x2) + (pos.Y - y2) * (pos.Y - y2));
        }

        // enables the player to move with angles
        public void AngleMath(float speed2)
        {
            angle2 = (angle * (float)Math.PI / 180);
            speed = speed2;
            scale_x = (float)Math.Cos(angle2);
            scale_y = (float)Math.Sin(angle2);
            veclocity_x = (speed * scale_x);
            veclocity_y = (speed * scale_y);
        }
        // makes the object aim at x2, y2
        public void MathAim(float speed2, float x2, float y2)
        {
            angle = (float)Math.Atan2(y2 - pos.Y, x2 - pos.X);
            speed = speed2;
            veclocity_x = (speed * (float)Math.Cos(angle));
            veclocity_y = (speed * (float)Math.Sin(angle));
        }

        // makes it easier to draw a sprite
        public void SetSpriteCoords(int imx2, int imy2)
        {
            imx = imx2;
            imy = imy2;
        }

        public void SetSize(int w2, int h2)
        {
            width = w2;
            height = h2;
        }

        public void DrawSprite(SpriteBatch spriteBatch, Texture2D spritesheet, float ang2)
        {
            spriteBatch.Draw(spritesheet, pos, new Rectangle(imx, imy, width, height), Color.White, ang2, new Vector2(width / 2, height / 2), 1.0f, SpriteEffects.None, 0);
        }
    }
}
