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
    class enemy:objects
    {
        public int lifeTime;
        public int type;
        public float shootingRange;
        public int firerate;
        public int maxFirerate;
        public int walkCount;
        public Vector2 walkTarget;
        public int changeTarget;

        public enemy(Vector2 pos2, Vector2 posAim, int type2)
        {
            Random random = new Random();
            type = type2;
            pos = pos2;
            SetSize(24, 24);
            switch (type)
            {
                case 1:
                    MathAim(5, posAim.X, posAim.Y);
                    hp = 1;
                    SetSpriteCoords(1, Frame(2));
                    break;
                case 2:
                    maxFirerate = random.Next(128, 128 * 2);
                    walkCount = random.Next(50);
                    shootingRange = random.Next(50, 200);
                    hp = 3;
                    SetSpriteCoords(1, Frame(3));
                    break;
                case 3:
                    MathAim(1, posAim.X, posAim.Y);
                    hp = 3;
                    SetSpriteCoords(1, Frame(4));
                    break;
                case 4:
                    MathAim(5, posAim.X, posAim.Y);
                    maxFirerate = random.Next(64 *2, 64 * 4);
                    shootingRange = random.Next(50, 200);
                    hp = 1;
                    SetSpriteCoords(1, Frame(5));
                    break;
            }
        }
        public void Movment(Vector2 posAim)
        {
            Random random = new Random();
            switch (type)
            {
                case 1:
                    pos.X += veclocity_x;
                    pos.Y += veclocity_y;
                    break;
                case 2:
                    MathAim(1, posAim.X, posAim.Y);
                    if (DistanceTo(posAim.X, posAim.Y) >= shootingRange)
                    {
                        walkCount += 1;
                        if (walkCount >= 100)
                        {
                            pos.X += veclocity_x;
                            pos.Y += veclocity_y;
                        }
                        if (walkCount >= random.Next(150, 200))
                        {
                            walkCount = random.Next(50);
                        }
                    }
                    break;
                case 3:
                    pos.X += veclocity_x;
                    pos.Y += veclocity_y;
                    break;
                case 4:
                    MathAim(1, posAim.X, posAim.Y);
                    if (DistanceTo(posAim.X, posAim.Y) >= shootingRange)
                    {
                        pos.X += veclocity_x;
                        pos.Y += veclocity_y;
                    }
                    break;
            }
        }
        public void Attacking(List<enemyBullet> enemyBullets, Vector2 posAim, SoundEffect shootSfx)
        {
            // planes don't shoot 
            if (type != 1)
            {
                firerate += 1;
                if (firerate >= maxFirerate)
                {
                    if (type != 3)
                    {
                        if (DistanceTo(posAim.X, posAim.Y) <= shootingRange)
                        {
                            enemyBullets.Add(new enemyBullet(pos, posAim, 2));
                            shootSfx.Play();
                        }
                    }
                    firerate = 0;
                }
            }
        }
        public void CheckHealth(Vector2 playerPos2, player player, List<bullet> bullets, ref int score, List<explosion> explosions, List<particle> particles, SoundEffect explosionSfx, SoundEffect splashSfx)
        {
            Random random = new Random();
            // diffrent sizes of the hitboxes
            Rectangle enemyC;
            if (type != 4)
            {
                enemyC = new Rectangle((int)pos.X-12, (int)pos.Y-12, width, height);
            }
            else
            {
                enemyC = new Rectangle((int)pos.X - 9 / 2, (int)pos.Y - 5, 9, 10);
            }
            Rectangle playerC = new Rectangle((int)playerPos2.X-12, (int)playerPos2.Y-12, 24, 24);
            foreach (bullet b in bullets)
            {
                Rectangle bulletC = new Rectangle((int)b.pos.X-2, (int)b.pos.Y-2, 4, 4);
                if (bulletC.Intersects(enemyC))
                {
                    b.destroy = true;
                    hp -= 1;
                    if (hp <= 0)
                    {
                        switch (type)
                        {
                            case 1:
                                score += 1000;
                                break;
                            case 2:
                                score += 2000;
                                break;
                            case 3:
                                score += 3000;
                                break;
                            case 4:
                                score += 500;
                                break;
                        }
                    }
                }
            }
            if (enemyC.Intersects(playerC))
            {
                if (type != 4)
                {
                    // if it's not a footsoldier the player loses one hp
                    player.hit = true;
                    player.hp -= 1;
                    hp = 0;
                }
                else
                {
                    // if the player drives over a solder he does not lose a hp 
                    hp = 0;
                    score += 5000;
                }
            }
            if (type == 1 || type == 3)
            {
                //wings weak, missiles are heavy, there's oil driping already, moms spghetti
                // Don't want to waste memoery on planes/cars just driving into the void
                lifeTime += 1;
                if (lifeTime >= 128 * 16)
                {
                    destroy = true;
                }
            }
            if (hp <= 0)
            {
                if (type != 4)
                {
                    // if it's not a footsoldier spawn an explosion and grey particles
                    explosions.Add(new explosion(pos));
                    explosionSfx.Play();
                    for (int i = 0; i < 50; i++)
                    {
                        particles.Add(new particle(pos, random.Next(10, 20), 1, random.Next(360), "grey"));
                    }
                }
                else
                {
                    // when a footsoldier dies spawn bloods
                    splashSfx.Play();
                    for (int i = 0; i < 30; i++)
                    {
                        particles.Add(new particle(pos, random.Next(2, 7), 1, random.Next(360), "red"));
                    }
                }
                destroy = true;
            }
        }

    }
}
