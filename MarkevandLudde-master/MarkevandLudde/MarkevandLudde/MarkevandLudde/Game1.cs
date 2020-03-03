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
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public enum state
    {
        Start,
        Game,
        StageW, //Stage Win
        GameOver, //Win and loss
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Master master;
        public GameOver GO;
        public Texture2D block,brick,background,lava,darkness,trans;
        public state cState,oldState;
        public SpriteFont font, fontS, fontT;
        public int maxStage,timer,timer2;
        public Color shade;
        public KeyboardState Kb, oldKB;
        public GamePadState GP1, OldGP1, GP2, OldGP2;
        public bool change;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteBatch = new SpriteBatch(GraphicsDevice);
            maxStage = 7;
            shade = new Color(0, 0, 0, 200);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            block = Content.Load<Texture2D>("white");
            brick = this.Content.Load<Texture2D>("Textures and Fonts/GameOverBrick");
            font = this.Content.Load<SpriteFont>("Textures and Fonts/font");
            background = this.Content.Load<Texture2D>("Textures and Fonts/Background");
            lava = this.Content.Load<Texture2D>("Textures and Fonts/Lava");
            darkness = this.Content.Load<Texture2D>("Textures and Fonts/Darkness");
            trans = this.Content.Load<Texture2D>("Textures and Fonts/Trans");
            fontS = this.Content.Load<SpriteFont>("Textures and Fonts/fontS");
            fontT = this.Content.Load<SpriteFont>("Textures and Fonts/Title");
            master = new Master(this, this.spriteBatch);
            oldState = state.Start;
            cState = state.Start;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed||kb.IsKeyDown(Keys.Escape)|| GamePad.GetState(PlayerIndex.Two).Buttons.Back==ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            kb = Keyboard.GetState();
            GP1 = GamePad.GetState(PlayerIndex.One);
            GP2 = GamePad.GetState(PlayerIndex.Two);

            switch (cState)
            {
                case state.Start:
                    if ((kb.IsKeyDown(Keys.Space) && !oldKB.IsKeyDown(Keys.Space)) ||
                        (GP1.IsButtonDown(Buttons.Start) && !OldGP1.IsButtonDown(Buttons.Start)) ||
                        (GP2.IsButtonDown(Buttons.Start) && !OldGP2.IsButtonDown(Buttons.Start)))
                        cState = state.Game;
                    break;
                case state.Game:
                    if ((kb.IsKeyDown(Keys.Space) && !oldKB.IsKeyDown(Keys.Space)) ||
                        (GP1.IsButtonDown(Buttons.Start) && !OldGP1.IsButtonDown(Buttons.Start)) ||
                        (GP2.IsButtonDown(Buttons.Start) && !OldGP2.IsButtonDown(Buttons.Start)))
                        master.pause = !master.pause;
                    master.Update(gameTime);
                    break;
                case state.StageW:
                    if (!change)
                    {
                        timer2 = timer + 180;
                        change = true;
                    }
                    if (master.stage < maxStage && timer2 == timer)
                    {
                        master.stage++;
                        change = false;
                        cState = state.Game;
                    }

                    else if (master.stage == maxStage && timer2 == timer)
                    {
                        GO = new GameOver(0, this);
                        cState = state.GameOver;
                    }
                    else if(timer2 == timer)
                    {
                        GO = new GameOver(0, this);
                        cState = state.GameOver;
                    }
                    break;
                case state.GameOver:
                    GO.Update(gameTime);
                    break;
            }
            oldState = cState;
            timer++;

            oldKB = kb;
            OldGP1 = GP1;
            OldGP2 = GP2;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.Draw(darkness, new Rectangle(0, 0, 800, 480), new Color(20, 20, 20, 20));
            switch (cState)
            {
                case state.Start:
                    spriteBatch.DrawString(fontT, "Markev & Ludde", new Vector2(268, 173), shade);
                    spriteBatch.DrawString(fontT, "Markev & Ludde", new Vector2(265, 170), Color.Red);
                    spriteBatch.DrawString(fontT, "Press Space/Start to Play", new Vector2(163, 223), shade);
                    spriteBatch.DrawString(fontT, "Press Space/Start to Play", new Vector2(160, 220), Color.Red);
                    spriteBatch.DrawString(fontT, "Press Escape/Back to Exit", new Vector2(163, 268), shade);
                    spriteBatch.DrawString(fontT, "Press Escape/Back to Exit", new Vector2(160, 265), Color.Red);
                    spriteBatch.DrawString(fontS, "Press Space to Pause During Gameplay", new Vector2(217, 332), shade);
                    spriteBatch.DrawString(fontS, "Press Space to Pause During Gameplay", new Vector2(215, 330), Color.Red);
                    break;
                case state.Game:
                    master.Draw();
                    break;
                case state.StageW:
                    if (master.stage == maxStage)
                    {
                        GO = new GameOver(0, this);
                        GO.Draw(spriteBatch);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, "Level " + master.stage + " Complete", new Vector2(263, 183), shade);
                        spriteBatch.DrawString(font, "Level " + master.stage + " Complete", new Vector2(260, 180), Color.Red);
                    }
                    break;
                case state.GameOver:
                    GO.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Texture2D Getblock()
        {
            return block;
        }
    }
}
