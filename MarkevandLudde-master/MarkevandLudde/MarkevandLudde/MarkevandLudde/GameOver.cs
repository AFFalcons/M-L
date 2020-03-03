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
    public class GameOver
    {
        //Texture2D text = Content.Load<Texture2D>("Textures and Fonts/GameOverBrick");

        int condition;
        Game1 g;

        /*
         * 0 - Win 
         * 1 - LivesM
         * 2 - LivesL
         * 3 - LivesB
         */
        public GameOver(int con, Game1 g)
        {
            condition = con;
            this.g = g;
        }

        public void Update(GameTime gt)
        {
            KeyboardState kb = Keyboard.GetState();
            GamePadState gp = GamePad.GetState(PlayerIndex.One);
            GamePadState gpTwo = GamePad.GetState(PlayerIndex.Two);

            if(kb.IsKeyDown(Keys.Space)||gp.IsButtonDown(Buttons.Start)||gpTwo.IsButtonDown(Buttons.Start))
            {
                g.master = new Master(g, g.spriteBatch);
                g.cState = state.Game;
            }

            if (kb.IsKeyDown(Keys.Escape) || gp.IsButtonDown(Buttons.Back) || gpTwo.IsButtonDown(Buttons.Back))
                g.Exit();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //Fix This
            if (condition == 0)
            {
                spriteBatch.DrawString(g.font, "You Win", new Vector2(338, 188), g.shade);
                spriteBatch.DrawString(g.font, "You Win", new Vector2(335, 185), Color.Red);
            }
            else
            {
                spriteBatch.DrawString(g.font, "Game Over", new Vector2(318, 188), g.shade);
                spriteBatch.DrawString(g.font, "Game Over", new Vector2(315, 185), Color.Red);
            }

            spriteBatch.DrawString(g.font, "Press Space or Start to Restart", new Vector2(148, 228), g.shade);
            spriteBatch.DrawString(g.font, "Press Space or Start to Restart", new Vector2(145, 225), Color.Red);
            spriteBatch.DrawString(g.font, "Press Escape or Back to Exit", new Vector2(178, 268), g.shade);
            spriteBatch.DrawString(g.font, "Press Escape or Back to Exit", new Vector2(175, 265), Color.Red);
        }

        void reset()
        {

        }
    }
}
