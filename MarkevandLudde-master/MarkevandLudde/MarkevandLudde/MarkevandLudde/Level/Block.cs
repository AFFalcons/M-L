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

namespace MarkevandLudde
{
    public enum Type
    {
        Start,End,Platform,Wall
    }

    public class Block
    {
        public Rectangle rec;
        public Texture2D pic;
        public Type type;
        public Color color;
        Master ms;

        public Block(Rectangle r,Texture2D p, int t, Master ms)
        {
            rec = r;
            pic = p;
            this.ms = ms;
            /*
             * 0 - Start 
             * 1 - End
             * 2 - Platform
             * 3 - Wall
             */
            switch (t) 
            {
                case 0:
                    type = Type.Start;
                    color = Color.Black;
                    break;
                case 1:
                    type = Type.End;
                    color = Color.Black;
                    break;
                case 2:
                    type = Type.Platform;
                    color = new Color(60, 60, 60);
                    break;
                case 3:
                    type = Type.Wall;
                    color = new Color(60, 60, 60);
                    break;
                default:
                    color = new Color(60, 60, 60);
                    break;
            }
        }

        public void setColor(Color c)
        {
            this.color = c;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, new Rectangle(rec.X, rec.Y + ms.yOff, rec.Width, rec.Height), color);
        }
    }
}
