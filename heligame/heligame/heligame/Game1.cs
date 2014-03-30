/*
 * Tom Leonardsson
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace heligame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 640;
        }

        int level;
        // so that I can display "get ready!" in the begining
        int readyCounter;
        float levelCount;
        int highscore;
        int menuDelay;
        player player = new player();
        bool gameover;
        // counts up untill the game goes back to the menu if the game over bool is true
        int gameoverCount;
        List<powerup> powerUps = new List<powerup>();
        List<bullet> bullets = new List<bullet>();
        List<enemy> enemies = new List<enemy>();
        List<enemyBullet> enemyBullets = new List<enemyBullet>();
        List<explosion> explosions = new List<explosion>();
        List<particle> particles = new List<particle>();
        spawnManager spawnManager = new spawnManager(); 
        string gameState = "menu";
        protected override void Initialize()
        {
            StreamReader highscoreFileReader = new StreamReader("highscore.hi", Encoding.UTF8);
            highscore = int.Parse(highscoreFileReader.ReadToEnd());
            highscoreFileReader.Close();
            readyCounter = 0;
            enemyBullets.Clear();
            bullets.Clear();
            explosions.Clear();
            enemies.Clear();
            particles.Clear();
            powerUps.Clear();
            player = new player();
            gameover = false;
            gameoverCount = 0;
            menuDelay = 0;
            level = 1;
            levelCount = 10;
            base.Initialize();
        }

        Texture2D spritesheet;
        Texture2D menu;
        SoundEffect shootSfx;
        SoundEffect explosionSfx;
        SoundEffect splashSfx;
        SoundEffect powerUpSfx;
        SpriteFont font;
        SpriteFont fontBig;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>("spritesheet");
            shootSfx = Content.Load<SoundEffect>("shoot_sfx");
            splashSfx = Content.Load<SoundEffect>("splash_sfx");
            explosionSfx = Content.Load<SoundEffect>("explosion_sfx");
            powerUpSfx = Content.Load<SoundEffect>("powerUp_sfx");
            font = Content.Load<SpriteFont>("font");
            fontBig = Content.Load<SpriteFont>("fontBig");
            menu = Content.Load<Texture2D>("menu");
        }

        protected override void UnloadContent()
        {
            
        }

        public void checkHighscore()
        {
            if (player.score >= highscore)
            {
                // if the player beats the highscore, the highscore is replaced with the player's score
                highscore = player.score;
                System.IO.StreamWriter highscoreFile = new System.IO.StreamWriter("highscore.hi");
                highscoreFile.WriteLine(highscore.ToString());
                highscoreFile.Close();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            checkHighscore();
            if (keyboard.IsKeyDown(Keys.F1))
            {
                graphics.ToggleFullScreen();
            }

            if(keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            switch (gameState)
            {
                case "menu":
                    menuDelay += 1;
                    if (keyboard.IsKeyDown(Keys.Space) || gamepad.Buttons.A == ButtonState.Pressed)
                    {
                        if (menuDelay >= 32)
                        {
                            Initialize();
                            gameState = "game";
                        }
                    }
                    break;
                case "game":
                    if (readyCounter <= 64*3)
                    {
                        readyCounter += 1;
                    }
                    // spawn enemies while the player is not dead
                    if (!gameover)
                    {
                        spawnManager.Spawn(level, enemies, powerUps, player.pos);
                        levelCount -= 0.01f;
                        if (levelCount <= 0)
                        {
                            level += 1;
                            levelCount = 10 * level;
                        }
                    }
                    else
                    {
                        gameoverCount += 1;
                        if (gameoverCount >= 128*2)
                        {
                            gameState = "menu";
                        }
                        if (gameoverCount == 50)
                        {
                            explosions.Add(new explosion(player.pos));
                        }
                        player.accelerating = false;
                    }
                    player.Input(bullets, particles, shootSfx);
                    player.Update(gameover);
                    player.CheckHealth(ref gameover, particles);

                    foreach (powerup pu in powerUps)
                    {
                        pu.Update(player, powerUpSfx);
                    }
                    foreach(explosion ex in explosions)
                    {
                        ex.Animation(particles);
                    }
                    foreach (enemyBullet eb in enemyBullets)
                    {
                        eb.Movment();
                        eb.Update(player);
                    }
                    foreach (bullet b in bullets)
                    {
                        b.Movment();
                    }
                    foreach (particle p in particles)
                    {
                        p.Movment();
                    }
                    foreach (enemy e in enemies)
                    {
                        e.Movment(player.pos);
                        e.CheckHealth(player.pos, player, bullets, ref player.score, explosions, particles, explosionSfx, splashSfx);
                        e.Attacking(enemyBullets, player.pos, shootSfx);
                    }
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].destroy)
                        {
                            enemies.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        if (bullets[i].destroy)
                        {
                            bullets.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < particles.Count; i++)
                    {
                        if (particles[i].destroy)
                        {
                            particles.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < enemyBullets.Count; i++)
                    {
                        if (enemyBullets[i].destroy)
                        {
                            enemyBullets.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < explosions.Count; i++)
                    {
                        if (explosions[i].destroy)
                        {
                            explosions.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < powerUps.Count; i++)
                    {
                        if (powerUps[i].destroy)
                        {
                            powerUps.RemoveAt(i);
                        }
                    }
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Random random = new Random();
            MouseState mouse = Mouse.GetState();
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            switch (gameState)
            {
                case "menu":
                    spriteBatch.Draw(menu, new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(font, "Hi-Score: " + highscore.ToString(), new Vector2(450, 10), Color.White);
                    break;
                case "game":
                    foreach (particle p in particles) { p.DrawSprite(spriteBatch, spritesheet, 0); }
                    player.DrawSprite(spriteBatch, spritesheet, player.angle * (float)Math.PI / 180);
                    foreach (bullet b in bullets) { b.DrawSprite(spriteBatch, spritesheet, b.angle); }
                    foreach (enemy e in enemies) { e.DrawSprite(spriteBatch, spritesheet, e.angle); }
                    foreach (enemyBullet eb in enemyBullets) { eb.DrawSprite(spriteBatch, spritesheet, 0); }
                    foreach (explosion ex in explosions) { ex.DrawSprite(spriteBatch, spritesheet, 0); }
                    foreach (powerup pu in powerUps) { pu.DrawSprite(spriteBatch, spritesheet, 0); }
                    spriteBatch.DrawString(font, "Score: " + player.score.ToString(), new Vector2(10, 10), Color.White);
                    spriteBatch.DrawString(font, "Level: " + level.ToString(), new Vector2(10, 34), Color.White);
                    spriteBatch.DrawString(font, "Next Level In: " + Math.Round(levelCount, 0), new Vector2(100, 34), Color.DeepPink);
                    if (readyCounter <= 64 * 3)
                    {
                        spriteBatch.DrawString(fontBig, "get ready!", new Vector2(230, 240 - 24), Color.SpringGreen); 
                    }
                    if (player.gunType != 1)
                    {
                        spriteBatch.DrawString(font, "Power Down In: " +player.powerDownCount, new Vector2(100, 54), Color.Violet);
                    }
                    spriteBatch.DrawString(font, "Hi-Score: " + highscore.ToString(), new Vector2(450, 10), Color.White);
                    if (player.hp == 3)
                    {
                        spriteBatch.Draw(spritesheet, new Vector2(10, 50), new Rectangle(1, 176, 48, 24), Color.Pink);
                    }
                    if (player.hp == 2)
                    {
                        spriteBatch.Draw(spritesheet, new Vector2(10, 50), new Rectangle(1, 176, 32, 24), Color.Tomato);
                    }
                    if (player.hp == 1)
                    {
                        spriteBatch.Draw(spritesheet, new Vector2(10, 50), new Rectangle(1, 176, 16, 24), Color.DarkRed);
                    }
                    
                    if (gameover)
                    {
                        if (gameoverCount >= 20)
                        {
                            spriteBatch.DrawString(fontBig, "game over!", new Vector2(230, 240 - 24), Color.Gold);
                        }
                        if (gameoverCount >= 50)
                        {
                            spriteBatch.DrawString(fontBig, "Score: " + player.score.ToString(), new Vector2(230, 240 + 24 * 3), Color.Green);
                        }
                        if (gameoverCount >= 80)
                        {
                            spriteBatch.DrawString(fontBig, "Highscore: " + highscore.ToString(), new Vector2(230, 250 + 24 * 4), Color.Blue);
                            if (player.score == highscore && player.score != 0)
                            {
                                spriteBatch.DrawString(fontBig, "new highscore!", new Vector2(330, 290 + 24 * 4), Color.YellowGreen);
                            }
                        }
                    }
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
