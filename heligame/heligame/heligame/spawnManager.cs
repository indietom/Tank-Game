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
    class spawnManager
    {
        public int spawnEnemyCount;
        public int spawnPowerUpCount;
        public int side;

        public void Spawn(int level, List<enemy> enemies, List<powerup> powerUps, Vector2 posAim)
        {
            Random random = new Random();
            spawnEnemyCount += 1;
            spawnPowerUpCount += 1;
            if (spawnPowerUpCount >= 128 * 3 + level*10)
            {
                powerUps.Add(new powerup(new Vector2(random.Next(640-24), random.Next(480-24)), random.Next(1,3)));
                spawnPowerUpCount = 0;
            }
            if (spawnEnemyCount >= 64 * 3)
            {
                for (int i = 0; i < level + 1; i++)
                {
                    side = random.Next(1, 5);
                    switch (side)
                    {
                        case 1:
                            enemies.Add(new enemy(new Vector2(random.Next(-340,-100), random.Next(480)), posAim, random.Next(1, 5)));
                            break;
                        case 2:
                            enemies.Add(new enemy(new Vector2(random.Next(640), random.Next(-340, -100)), posAim, random.Next(1, 5)));
                            break;
                        case 3:
                            enemies.Add(new enemy(new Vector2(random.Next(740, 980), random.Next(480)), posAim, random.Next(1, 5)));
                            break;
                        case 4:
                            enemies.Add(new enemy(new Vector2(random.Next(640), random.Next(740, 980)), posAim, random.Next(1, 5)));
                            break;
                    }
                }
                spawnEnemyCount = 0;
            }
        }
    }
}
