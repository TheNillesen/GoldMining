using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GoldMiningString
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private static GameWorld instance;
        private List<GameObject> gameObjects;
        SpriteFont bFont;
        SpriteFont aFont;
        int number; // amount of workers
        Random rnd;
        bool canAddWorker;
        bool canDeleteWorker;
        bool isPaused;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        public SpriteFont BFont
        {
            get
            {
                return bFont;
            }
        }

        public Random Rnd
        {
            get
            {
                return rnd;
            }
        }

        public SpriteFont AFont
        {
            get
            {
                return aFont;
            }
        }

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 650;
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
            rnd = new Random();
            number = 0;
            canAddWorker = true;
            canDeleteWorker = true;
            gameObjects = new List<GameObject>();
            gameObjects.Add(new Mine(new Vector2(200, 240), "ore", 0.6f));
            gameObjects.Add(new Factory(new Vector2(670, 50), "factory", 0.7f));
            gameObjects.Add(new Ws(new Vector2(700, 500), "ws", 0.2f));
            for (int i = 0; i < 5; i++)
            {
                number++;
                gameObjects.Add(new Worker(new Vector2(rnd.Next(900, 1000), rnd.Next(240, 270)), "man", 0.3f, number.ToString()));
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            bFont = Content.Load<SpriteFont>("BFont");
            aFont = Content.Load<SpriteFont>("AFont");
            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(Content);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            AddWorker();
            DeleteWorker();
            StartStop();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);
            }
            spriteBatch.DrawString(bFont, "Workers: "+ number, new Vector2(10, 100), Color.Black);
            spriteBatch.DrawString(bFont, "[R] - to recruit worker", new Vector2(10, 200), Color.Black);
            spriteBatch.DrawString(bFont, "[F] - to fire worker", new Vector2(10, 230), Color.Black);
            spriteBatch.DrawString(bFont, "[P] - pause", new Vector2(10, 260), Color.Black);
            spriteBatch.DrawString(bFont, "[C] - resume game", new Vector2(10, 280), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void AddWorker()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && canAddWorker && gameObjects.Count <= 40 && Factory.GoldAmount >= 0)
            {
                GameObject go = new Worker(new Vector2(rnd.Next(900, 1000), rnd.Next(240, 270)), "man", 0.3f, number.ToString());
                go.LoadContent(Content);

                gameObjects.Add(go);
                number++;
                //Factory.GoldAmount -= 100;
                canAddWorker = false;

            }
            if (Keyboard.GetState().IsKeyUp(Keys.R))
            {
                canAddWorker = true;
            }

        }
        public void DeleteWorker()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F) && canDeleteWorker && number > 0)
            {
                (gameObjects[gameObjects.Count - 1] as Worker).WThread.Abort();
                gameObjects.RemoveAt(gameObjects.Count - 1);
                number--;
                canDeleteWorker = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.F))
            {
                canDeleteWorker = true;
            }
        }
        public void StartStop()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.C) && isPaused && number > 0)
            {
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                    {
                        (go as Worker).WThread = new Thread((go as Worker).Move);
                        (go as Worker).WThread.IsBackground = true;
                        (go as Worker).WThread.Start();
                    }
                }
                isPaused = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.P) && !isPaused && number > 0)
            {
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                        (go as Worker).WThread.Abort();
                }
                isPaused = true;
            }
        }
    }
}