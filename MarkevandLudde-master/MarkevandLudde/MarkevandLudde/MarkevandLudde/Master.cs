using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace MarkevandLudde
{
    public class Master
    {
        public Game1 game;
        SpriteBatch spriteBatch;
        public int yOff;
        public Level cLevel;
        public static string path;
        public int stage = 1;
        public int oldStage = 1;
        public bool pause;

        public Master(Game1 game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            yOff = 0;
            path = @"Content\Levels\Level_" + stage + ".txt";
            LoadLevel(path);
            pause = false;
        }

        public void Update(GameTime gameTime)
        {
            if(oldStage != stage)
            {
                path = @"Content\Levels\Level_" + stage + ".txt";
                LoadLevel(path);
                oldStage = stage;
            }

            if (cLevel != null)
                cLevel.Update(gameTime);

            cLevel.Map();

            if (cLevel.markev.lives == 0 && cLevel.ludde.lives == 0)
            {
                game.GO = new GameOver(3, game);
                game.cState = state.GameOver;
            }

            if (cLevel.markev.lives == 0)
            {
                game.GO = new GameOver(1, game);
                game.cState = state.GameOver;
            }

            if (cLevel.ludde.lives == 0)
            {
                game.GO = new GameOver(2, game);
                game.cState = state.GameOver;
            }
        }

        public void Draw()
        {
            cLevel.Draw(spriteBatch);
        }

        public void LoadLevel(string path)
        {
            List<Block> ll = new List<Block>();
            List<Player> pp = new List<Player>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int b = Convert.ToInt32(sr.ReadLine());
                    ll = new List<Block>(b);

                    for (int i = 0; i < 2; i++)
                    {
                        string line = sr.ReadLine();
                        string[] parts = line.Split(' ');

                        pp.Add(new Player(GetPl(0), new Rectangle(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]), Convert.ToInt32(parts[4])), 3, Convert.ToInt32(parts[0]), this));
                        
                    }

                    for (int i = 0; i < b; i++)
                    {
                        string line = sr.ReadLine();
                        string[] parts = line.Split(' ');

                        /*
                         * Index 0 Type of block
                         * 0 - Start 
                         * 1 - End
                         * 2 - Platform
                         * 3 - Wall
                         * 
                         * Index 1 - 4
                         * Position and size of block as a rectangle
                         */

                        ll.Add(new Block(new Rectangle(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]), Convert.ToInt32(parts[4])),
                            GetIMG(Convert.ToInt32(parts[0])), Convert.ToInt32(parts[0]), this));
                    }

                    pp[0].SetBlocks(ll);
                    pp[1].SetBlocks(ll);
                }

                cLevel = new Level(ll, pp, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("File: " + path + " can't be read.");
                Console.WriteLine("The Program can not load the level");
                Console.WriteLine(e.Message);
                game.Exit();
            }

            
        }

        public Texture2D GetPl(int x)
        {

            return game.block;
        }

        public Texture2D GetIMG(int x)
        {
            return game.block;
        }
    }
}
