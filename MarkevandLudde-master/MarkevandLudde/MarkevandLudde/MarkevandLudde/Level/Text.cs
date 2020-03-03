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
    //Class For text shown durring levels
    public class Text
    {

        Master ms;
        
        public Text(Master MS)
        {
            this.ms = MS;
        }

        public void Draw(SpriteBatch sb)
        {
            switch (ms.stage)
            {
                case 1:
                    sb.DrawString(ms.game.fontS, "To Control Ludde", new Vector2(27, 402), ms.game.shade);
                    sb.DrawString(ms.game.fontS, "To Control Ludde", new Vector2(25, 400), Color.Green);
                    sb.DrawString(ms.game.fontS, "To Control Markev", new Vector2(603, 402), ms.game.shade);
                    sb.DrawString(ms.game.fontS, "To Control Markev", new Vector2(601, 400), Color.Red);
                    sb.DrawString(ms.game.fontS, "Reach Here", new Vector2(351, 137), ms.game.shade);
                    sb.DrawString(ms.game.fontS, "Reach Here", new Vector2(349, 135), Color.Red);
                    sb.Draw(ms.game.trans, new Rectangle(75, 350, 57, 38), new Rectangle(0, 0, 38, 25), Color.White);
                    sb.Draw(ms.game.trans, new Rectangle(660, 350, 57, 38), new Rectangle(38, 0, 38, 25), Color.White);
                    break;
                case 2:
                    sb.DrawString(ms.game.fontT, "Go Up",new Vector2(353, 223 + ms.yOff), ms.game.shade);
                    sb.DrawString(ms.game.fontT, "Go Up", new Vector2(350, 220 + ms.yOff), Color.Red);
                    sb.DrawString(ms.game.fontS, "Screen will Follow", new Vector2(313, 263 + ms.yOff), ms.game.shade);
                    sb.DrawString(ms.game.fontS, "Screen will Follow", new Vector2(310, 260 + ms.yOff), Color.Red);
                    break;
                case 3:
                    sb.DrawString(ms.game.fontT, "Walls", new Vector2(353, 223 + ms.yOff), ms.game.shade);
                    sb.DrawString(ms.game.fontT, "Walls", new Vector2(350, 220 + ms.yOff), Color.Red);
                    sb.DrawString(ms.game.fontS, "Player will pass Through", new Vector2(283, 263 + ms.yOff), ms.game.shade);
                    sb.DrawString(ms.game.fontS, "Player will pass Through", new Vector2(280, 260 + ms.yOff), Color.Red);
                    break;
                default:
                    break;
            }
        }

    }
}
