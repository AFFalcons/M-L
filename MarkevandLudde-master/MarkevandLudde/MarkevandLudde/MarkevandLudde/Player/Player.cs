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
    public enum State
    {
        Standing, Running, Jumping, Dead
    }

    public enum Character
    {
        Markev, Ludde //0 is Markev, 1 is Ludde
    }

    public class Player
    {
        public Texture2D pic;
        public Rectangle rec;
        public State state;
        public State oldState;
        public int lives;
        public Character character;
        public Color color, otherColor;
        public bool isRight, isJumping;
        PlayerIndex index;
        public int dir, maxHeight, reverse, jumpHeight;
        Master ms;
        GamePadState OldGP;

        //Velocity
        private double maxVelX = 4; //Maximum horizontal speed is 10.0m/s
        private double maxVelY = 25;

        //Player Info
        public Vector2 vel; //Player velocity
        public Vector2 origin;

        //Player State
        public Vector2 pos; //Current position of the player
        public Vector2 oldPos; //Old position of the player
        public Vector2 init; //Player's initial position
        public bool grounded; //If the player is on the ground

        //Stage Info
        public Rectangle bounds; //Current position from origin
        public List<Block> blocks;

        public int timer;
        public int num;

        public Player(Texture2D p, Rectangle r, int l, int c, Master ms)
        {
            pic = p;
            rec = r;
            lives = l;
            state = State.Standing;
            oldState = State.Standing;
            pos = new Vector2(rec.X, rec.Y);
            jumpHeight = 100;
            this.ms = ms;

            switch (c)
            {
                case 0:
                    character = Character.Markev;
                    color = Color.Red;
                    otherColor = Color.Green;
                    index = PlayerIndex.One;
                    break;
                case 1:
                    character = Character.Ludde;
                    index = PlayerIndex.Two;
                    color = Color.Green;
                    otherColor = Color.Red;
                    break;
            }

            this.grounded = true;
            OldGP = GamePad.GetState(index);
            this.init = new Vector2(pic.Width / 2, pic.Height / 2);
            this.bounds = new Rectangle((int)Math.Round(pos.X - init.X) + rec.X, (int)Math.Round(pos.Y - init.Y) + rec.Y, rec.Width, rec.Height);
            this.origin = new Vector2(pic.Width / 2, pic.Height / 2);
        }

        public void SetBlocks(List<Block> b)
        {
            this.blocks = b;
        }

        public void Update(GameTime gt)
        {
            KeyboardState kb = Keyboard.GetState();
            GamePadState gp = GamePad.GetState(index);

            GetInput(kb, gp);
            if(!ms.pause)
                Physics();

            oldState = state; //Previous state of the player
        }

        public Vector2 Physics()
        {
            oldPos = pos;

            if (state == State.Jumping && !isJumping)
            {
                timer = 0;
                vel.Y = -10;
                isJumping = true;
            }

            if (state == State.Standing && !isJumping)
            {
                vel.X = 0;
                vel.Y = 0;
            }

            if (isJumping)
            {
                vel.Y += (float)(5.4 / 60) * timer;
                timer++;
                midJump();
            }

            vel.Y = MathHelper.Clamp(vel.Y, (float)-maxVelY, (float)maxVelY);
            vel.X = MathHelper.Clamp(vel.X, (float)-maxVelX, (float)maxVelX);

            pos.X += vel.X;
            pos.Y += vel.Y;

            //Console.WriteLine(state);

            for (int a = 0; a < blocks.Count(); a++)
                if (rec.Intersects(blocks[a].rec) && otherColor != blocks[a].color)
                    rec = new Rectangle((int)oldPos.X, (int)oldPos.Y, rec.Width, rec.Height);

            Collisions();

            if (state == State.Running && !isJumping)
                Under();

            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
            return pos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, new Rectangle(rec.X, rec.Y + ms.yOff, rec.Width, rec.Height), color);
        }

        private void Under()
        {
            Rectangle temp = new Rectangle(bounds.X, bounds.Bottom, bounds.Width, 480 - bounds.Bottom);
            bool fall = true;

            for (int a = 0; a < blocks.Count(); a++)
                if (temp.Intersects(blocks[a].rec) || grounded)
                    fall = false;

            if (fall)
            {
                isJumping = true;
                timer = 0;
            }
        }

        private void Collisions()
        {
            Rectangle localBounds = new Rectangle(0, 0, pic.Width, pic.Height);
            Rectangle bounds = new Rectangle((int)Math.Round(pos.X - origin.X) + localBounds.X, (int)Math.Round(pos.Y - origin.Y) + localBounds.Y, localBounds.Width, localBounds.Height);

            int left = (int)Math.Floor((float)bounds.Left / 10);
            int right = (int)Math.Ceiling((float)bounds.Right / 10) - 1;
            int top = (int)Math.Floor((float)bounds.Top / 10);
            int bottom = (int)Math.Ceiling((float)bounds.Bottom / 10) - 1;

            grounded = false;

            for (int a = 0; a < blocks.Count(); a++)
            {
                Vector2 centerP = new Vector2(rec.X + (rec.Width / 2), rec.Y + (rec.Height / 2));
                Vector2 centerB = new Vector2(blocks[a].rec.X + (blocks[a].rec.Width / 2), blocks[a].rec.Y + (blocks[a].rec.Height / 2));

                if (rec.Intersects(blocks[a].rec) && otherColor != blocks[a].color
                    && blocks[a].type!=Type.Wall)
                {
                    if (centerP.Y > centerB.Y || centerP.Y < centerB.Y)
                        vel.Y = 0;

                    int diffX = blocks[a].rec.X - (int)oldPos.X;
                    int diffY = Math.Abs(blocks[a].rec.Y - (int)oldPos.Y);

                    pos = new Vector2(oldPos.X, oldPos.Y + diffY - blocks[a].rec.Height);
                    isJumping = false;
                }

                else if (blocks[a].type == Type.Wall)
                {
                    centerP = new Vector2(rec.X + (rec.Width / 2), rec.Y + (rec.Height / 2));
                    centerB = new Vector2(blocks[a].rec.X + (blocks[a].rec.Width / 2), blocks[a].rec.Y + (blocks[a].rec.Height / 2));

                    if (rec.Intersects(blocks[a].rec) && otherColor == blocks[a].color)
                    {
                        vel.X = 0;

                        int diffX = blocks[a].rec.X - (int)oldPos.X;
                        int diffY = Math.Abs(blocks[a].rec.Y - (int)oldPos.Y);

                        pos = new Vector2(blocks[a].rec.Left, blocks[a].rec.Bottom);

                        if (centerP.X < centerB.X)
                            pos = new Vector2(blocks[a].rec.Left - rec.Width, oldPos.Y);

                        else if (centerP.X > centerB.X)
                            pos = new Vector2(blocks[a].rec.Right, oldPos.Y);

                        else if (centerP.Y > centerB.Y)
                            vel = Vector2.Zero;

                        isJumping = true;
                    }
                }

                else if (Math.Abs(bottom - blocks[a].rec.Y) < 3 && Math.Abs(centerB.X - centerP.X) < rec.Width / 2 + rec.Width / 2)
                    state = State.Standing;
            }
        }

        void midJump()
        {
            KeyboardState kb = Keyboard.GetState();
            GamePadState gp = GamePad.GetState(index);
            switch (character)
            {
                case Character.Markev:
                    if (kb.IsKeyDown(Keys.Left) || gp.IsButtonDown(Buttons.DPadLeft))
                    {
                        isRight = false;
                        dir = -1;
                        vel.X = dir * (float)maxVelX;
                    }

                    if ((kb.IsKeyDown(Keys.Right) || gp.IsButtonDown(Buttons.DPadRight)))
                    {
                        isRight = true;
                        dir = 1;
                        vel.X = dir * (float)maxVelX;
                    }
                    break;
                case Character.Ludde:
                    if (kb.IsKeyDown(Keys.A) || gp.IsButtonDown(Buttons.DPadLeft))
                    {
                        isRight = false;
                        dir = -1;
                        vel.X = dir * (float)maxVelX;
                    }

                    if ((kb.IsKeyDown(Keys.D) || gp.IsButtonDown(Buttons.DPadRight)))
                    {
                        isRight = true;
                        dir = 1;
                        vel.X = dir * (float)maxVelX;
                    }
                    break;
            }
        }

        public void GetInput(KeyboardState kb, GamePadState gp)
        {

            if (!ms.pause)
            {
                switch (character)
                {
                    case Character.Markev:
                        if (kb.IsKeyDown(Keys.Up) || gp.IsButtonDown(Buttons.DPadUp)||gp.IsButtonDown(Buttons.A))
                            state = State.Jumping;

                        else if (kb.IsKeyDown(Keys.Left) || gp.IsButtonDown(Buttons.DPadLeft))
                        {
                            state = State.Running;
                            isRight = false;
                            dir = -1;
                        }

                        else if ((kb.IsKeyDown(Keys.Right) || gp.IsButtonDown(Buttons.DPadRight)))
                        {
                            state = State.Running;
                            isRight = true;
                            dir = 1;
                        }

                        else
                            state = State.Standing;

                        break;

                    case Character.Ludde:
                        if (kb.IsKeyDown(Keys.W) || gp.IsButtonDown(Buttons.DPadUp)||gp.IsButtonDown(Buttons.A))
                            state = State.Jumping;

                        else if ((kb.IsKeyDown(Keys.A) || gp.IsButtonDown(Buttons.DPadLeft)))
                        {
                            state = State.Running;
                            isRight = false;
                            dir = -1;
                        }

                        else if ((kb.IsKeyDown(Keys.D) || gp.IsButtonDown(Buttons.DPadRight)))
                        {
                            state = State.Running;
                            isRight = false;
                            dir = 1;
                        }

                        else
                            state = State.Standing;

                        break;
                }
            }
            OldGP = gp;
        }
    }
}