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
using System.Text;
using System.IO;

namespace MarkevandLudde
{
    enum Stage
    {
        Regular,Boss,Timed
    }

    public class Level
    {
        public Player markev;
        public Player ludde;
        public Text LText; //Displays text durring level
        List<Block> blocks;
        List<Player> players;
        Player tempM, tempL;
        public bool death;
        public int count;
        Lava lava;
        Master MS;

        public Level(List<Block> bl,List<Player> pl, Master ms)
        {
            blocks = bl;
            markev = pl[0];
            ludde = pl[1];
            lava = new Lava(ms.game.lava);
            players = new List<Player>();
            tempL = new Player(ludde.pic,ludde.rec,ludde.lives,1, ms);
            tempM = new Player(markev.pic, markev.rec, markev.lives, 0, ms);
            LText = new Text(ms);
            this.MS = ms;
            death = false;
            count = 0;
        }

        public void Update(GameTime gt)
        {
            if (!death)
            {
                markev.Update(gt);
                ludde.Update(gt);
                intersect();
                checkWin();
            }

            if (death)
            {
                count++;
                if (count >= 100)
                {
                    death = false;
                    count = 0;
                }
            }
            //Console.WriteLine(markev.rec);
            //Console.WriteLine(ludde.rec);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws Text to screen
            LText.Draw(spriteBatch);

            //Draws ellements that interact
            for (int a = 0; a < blocks.Count; a++)
                blocks[a].Draw(spriteBatch);
            markev.Draw(spriteBatch);
            ludde.Draw(spriteBatch);
            lava.Draw(spriteBatch);

            //Pause Menu
            if (MS.pause)
            {
                spriteBatch.Draw(MS.game.block, new Rectangle(0, 0, 800, 480), new Color(0, 0, 0, 35));
                spriteBatch.DrawString(MS.game.font, "- Game Paused -", new Vector2(273, 53), MS.game.shade);
                spriteBatch.DrawString(MS.game.font, "- Game Paused -", new Vector2(270, 50), Color.Red);
                spriteBatch.DrawString(MS.game.font, "Press Space/Start to Continue", new Vector2(163, 83), MS.game.shade);
                spriteBatch.DrawString(MS.game.font, "Press Space/Start to Continue", new Vector2(160, 80), Color.Red);
            }

            //HUD
            spriteBatch.DrawString(MS.game.fontS, "Level: " + MS.stage + "\nMarkev Lives: " + markev.lives + "\nLudde Lives: " + ludde.lives, new Vector2(2, 2), MS.game.shade);
            spriteBatch.DrawString(MS.game.fontS, "Level: " + MS.stage + "\nMarkev Lives: " + markev.lives + "\nLudde Lives: " + ludde.lives, new Vector2(0, 0), Color.Red);
        }

        public void setPlayer(List<Player> pl)
        {
            players = pl;
        }

        void intersect()
        {
            for (int a = 0; a < blocks.Count; a++)
            {
                if (markev.rec.Intersects(blocks[a].rec) && (blocks[a].type == Type.Platform|| blocks[a].type == Type.Wall))
                    if (blocks[a].color != Color.Green)
                        blocks[a].color = Color.Red;
                if (ludde.rec.Intersects(blocks[a].rec) && (blocks[a].type == Type.Platform || blocks[a].type == Type.Wall))
                    if (blocks[a].color != Color.Red)
                        blocks[a].color = Color.Green;
            }
        }

        //Pass a Stage
        void checkWin()
        {
            for(int a=0;a<blocks.Count;a++)
                if(blocks[a].type==Type.End)
                {
                    if(Math.Abs(blocks[a].rec.X+blocks[a].rec.Width/2-markev.rec.X)<blocks[a].rec.Width/2+12 && 
                        Math.Abs(blocks[a].rec.Y + blocks[a].rec.Height / 2 - markev.rec.Y) < blocks[a].rec.Height/ 2+12&& 
                        Math.Abs(blocks[a].rec.X + blocks[a].rec.Width / 2 - ludde.rec.X) < blocks[a].rec.Width / 2 +12&&
                        Math.Abs(blocks[a].rec.Y + blocks[a].rec.Height / 2 - ludde.rec.Y) < blocks[a].rec.Height / 2+12)
                        MS.game.cState = state.StageW;
                }
        }

        void resetLevel(string path)
        {

            MS.yOff = 0;
            for (int a = 0; a < blocks.Count; a++)
                if (blocks[a].type == Type.Platform || blocks[a].type == Type.Wall)
                    blocks[a].color = new Color(60, 60, 60);
                else
                    blocks[a].color = new Color(0, 0, 0);
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int b = Convert.ToInt32(sr.ReadLine());

                    string line = sr.ReadLine();
                    string[] parts = line.Split(' ');

                    markev.rec = new Rectangle(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]), Convert.ToInt32(parts[4]));
                    markev.pos = new Vector2(markev.rec.X, markev.rec.Y);
                    markev.oldPos = markev.pos;
                    markev.isJumping = false;
                    markev.state = State.Standing;
                    markev.vel = Vector2.Zero;

                    line = sr.ReadLine();
                    parts = line.Split(' ');

                    ludde.rec = new Rectangle(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]), Convert.ToInt32(parts[4]));
                    ludde.pos = new Vector2(ludde.rec.X, ludde.rec.Y);
                    ludde.oldPos = ludde.pos;
                    ludde.isJumping = false;
                    ludde.state = State.Standing;
                    ludde.vel = Vector2.Zero;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("File: " + path + " can't be read.");
                Console.WriteLine("The Program can not load the level");
                Console.WriteLine(e.Message);
            }
            markev.blocks = blocks;
            ludde.blocks = blocks;
        }

        //General Checks
        public void Map()
        {
            if (markev.rec.Y <= 20 - MS.yOff)
                MS.yOff += 4;
            if (ludde.rec.Y <= 20 - MS.yOff)
                MS.yOff += 4;
            if (markev.rec.Y >= lava.rec.Y - MS.yOff)
            {
                resetLevel(Master.path);
                markev.lives--;
            }
            if (ludde.rec.Y >= lava.rec.Y - MS.yOff)
            {
                resetLevel(Master.path);
                ludde.lives--;
            }
        }
   }
}
