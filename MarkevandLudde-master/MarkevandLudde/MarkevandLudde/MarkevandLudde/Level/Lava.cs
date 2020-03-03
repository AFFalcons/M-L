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

namespace MarkevandLudde
{
    class Lava
    {
        public Rectangle rec;
        public Texture2D pic;

        public Lava(Texture2D p)
        {
            rec = new Rectangle(0, 440, 800, 50);
            pic = p;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, rec, Color.White);
        }
    }
}