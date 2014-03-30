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
    class player:objects
    {
        public bool inputActive;
        public bool hit;
        public int fireRate;
        // contoll what type of gun the player has
        public int gunType;
        public bool accelerating;
        public int score;
        // controlles how much the controller vibrates
        public float vibrateForce;
        public int powerDownCount;

        public player()
        {
            maxAccel = 4;
            minAccel = -4;
            pos = new Vector2(320, 240);
            SetSpriteCoords(1, 1);
            gunType = 1;
            SetSize(24, 24);
            inputActive = true;
            hp = 3;
            score = 0;
            powerDownCount = 128 * 3;
        }
        public void Update(bool gameover)
        {
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            // if the player is accelerating or reversing the contoller vibrates
            if (gamepad.Triggers.Right == 1.0f || gamepad.Triggers.Left == 1.0f)
            {
                if(!gameover)
                    vibrateForce = 0.2f;
            }
            else
            {
                vibrateForce = 0;
            }
            GamePad.SetVibration(PlayerIndex.One, vibrateForce, vibrateForce);
            
            if (angle >= 360 || angle <= -360)
            {
                angle = 0;
            }
            AngleMath(accel);
            if (fireRate >= 1)
            {
                fireRate += 1;
                if (fireRate >= 24)
                {
                    fireRate = 0;
                }
            }
            if (gunType != 1)
            {
                powerDownCount -= 1;
                if (powerDownCount <= 0)
                {
                    gunType = 1;
                    powerDownCount = 128 * 3;
                }
            }
            // the player is constatly being moved foward but the speed goes down if the player is not driving foward
            pos.X += veclocity_x;
            pos.Y += veclocity_y;
            // Decrese or increse the players speed so that the player stops
            if (!accelerating)
            {
                if (accel > 0)
                    accel -= 0.01f;
                else
                    accel += 0.01f;
            }
            // keep the player on screen
            if (pos.X - 12 <= 0)
            {
                if (accel >= 0.01f)
                    pos.X += accel;
                else
                    pos.X -= accel;
                accel = 0;
            }
            if (pos.X + 12 >= 640)
            {
                if (accel >= 0.01f)
                    pos.X -= accel;
                else
                    pos.X += accel;
                accel = 0;
            }
            if (pos.Y + 12 >= 480)
            {
                if (accel >= 0.01f)
                    pos.Y -= accel;
                else
                    pos.Y += accel;
                accel = 0;
            }
            if (pos.Y - 12 <= 0)
            {
                if (accel >= 0.01f)
                    pos.Y += accel;
                else
                    pos.Y -= accel;
                accel = 0;
            }
        }
        public void CheckHealth(ref bool gameover, List<particle> particles)
        {
            // the player sholdn't move when it's dead
            Random random = new Random();
            if (hp <= 0)
            {
                inputActive = false;
                gameover = true;
            }
            if (hit)
            {
                for(int i = 0; i < 20; i++)
                    particles.Add(new particle(new Vector2(pos.X + random.Next(-12,12), pos.Y+random.Next(-12,12)), random.Next(5,15), 1, random.Next(360), "red")); 
                hit = false;
            }
        }
        public void Input(List<bullet> bullets, List<particle> particles, SoundEffect shootSfx)
        {
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            Random random = new Random();

            if (inputActive)
            {
                if (keyboard.IsKeyDown(Keys.Space) || gamepad.Buttons.A == ButtonState.Pressed)
                {
                    if (gunType == 1 && fireRate <= 0)
                    {
                        bullets.Add(new bullet(pos, 200, angle));
                        shootSfx.Play();
                        fireRate = 1;
                    }
                    if (gunType == 2 && fireRate <= 0)
                    {
                        bullets.Add(new bullet(pos, 200, angle-45));
                        bullets.Add(new bullet(pos, 200, angle));
                        bullets.Add(new bullet(pos, 200, angle+45));
                        shootSfx.Play();
                        fireRate = 1;
                    }
                    if (gunType == 3 && fireRate <= 0)
                    {
                        bullets.Add(new bullet(pos, 200, angle));
                        shootSfx.Play();
                        fireRate = 12;
                    }
                }
                if (keyboard.IsKeyDown(Keys.W) || gamepad.Triggers.Right == 1.0f)
                {
                    if (accel <= maxAccel)
                        accel += 0.05f;
                    accelerating = true;
                }
                else
                {
                    accelerating = false;
                }
                if (keyboard.IsKeyDown(Keys.S) || gamepad.Triggers.Left == 1.0f)
                {
                    if(accel >= minAccel)
                        accel -= 0.05f;
                    accelerating = true;
                }
                else
                {
                    accelerating = false;
                }
                if (keyboard.IsKeyDown(Keys.A) || gamepad.ThumbSticks.Left.X == -1.0f)
                {
                    angle -= 2f;
                }
                if (keyboard.IsKeyDown(Keys.D) || gamepad.ThumbSticks.Left.X == 1.0f)
                {
                    angle += 2f;
                }
            }
        }
    }
}
