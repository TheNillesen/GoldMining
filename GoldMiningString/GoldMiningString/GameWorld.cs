using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace GoldMiningString
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        Thread timerThread;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private static GameWorld instance;
        private List<GameObject> gameObjects;
        SpriteFont bFont;
        SpriteFont aFont;
        SpriteFont dFont;
        int number; // amount of workers
        Random rnd;
        bool canAddWorker;
        bool canDeleteWorker;
        bool canRestart;
        bool isPaused;
        bool firstStart;
        float min, sec;
        float deltaTime;
        bool playGame;
        Song backGroundSound;

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
        public SpriteFont DFont
        {
            get
            {
                return dFont;
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
            firstStart = true;
            isPaused = true;
            min = 15;
            sec = 0;
            number = 0;
            canAddWorker = true;
            canDeleteWorker = true;
            canRestart = true;
            playGame = true;
            timerThread = new Thread(UpdateTimer);
            timerThread.IsBackground = true;
            gameObjects = new List<GameObject>();
            gameObjects.Add(new Mine(new Vector2(200, 240), "ore", 0.6f));
            gameObjects.Add(new Factory(new Vector2(670, 50), "factory", 0.7f));
            gameObjects.Add(new Wc(new Vector2(700, 500), "ws", 0.2f));
            gameObjects.Add(new Canteen(new Vector2(450, 480), "canteen2", 0.5f));
            this.backGroundSound = Content.Load<Song>("Jazz");
            MediaPlayer.Play(backGroundSound);
            MediaPlayer.IsRepeating = true;

            // gameObjects.Add(new Bank(new Vector2(200, 400), "bank", 0.9f));
            for (int i = 0; i < 5; i++)
            {
                number++;
                gameObjects.Add(new Worker(new Vector2(rnd.Next(900, 1000), rnd.Next(260, 280)), "man", 0.3f, number.ToString()));
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
            dFont = Content.Load<SpriteFont>("DFont");
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
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (playGame)
            {
                //if (!isPaused)
                  //  UpdateTimer();
                AddWorker();
                DeleteWorker();
                StartStop();
            }
            RestartGame();

            base.Update(gameTime);
        }
        //public void BackGroundMusic()
        //{
        //    while()
        //    this.backGroundSound = Content.Load<Song>("Jazz");
        //    MediaPlayer.Play(backGroundSound);
        //    MediaPlayer.IsRepeating = true;

        
        //}



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
            spriteBatch.DrawString(bFont, "Workers: " + number, new Vector2(10, 100), Color.Black);
            spriteBatch.DrawString(bFont, "[A] - to recruit worker", new Vector2(10, 200), Color.Black);
            spriteBatch.DrawString(bFont, "[F] - to fire worker", new Vector2(10, 230), Color.Black);
            spriteBatch.DrawString(bFont, "[P] - pause", new Vector2(10, 260), Color.Black);
            spriteBatch.DrawString(bFont, "[S] - start / resume game", new Vector2(10, 280), Color.Black);
            spriteBatch.DrawString(bFont, "[esc] - exit game", new Vector2(10, 530), Color.Black);
            spriteBatch.DrawString(bFont, "[R] - restart game", new Vector2(10, 500), Color.Black);
            spriteBatch.DrawString(dFont, string.Format("Time left: {0:00}:{1:00}", min, sec), new Vector2(10, 30), Color.Red);
            if (firstStart)
            {
                spriteBatch.DrawString(bFont, "Buy workers and take care about the factory", new Vector2(400, 300), Color.Green);
                spriteBatch.DrawString(bFont, "To buy a worker costs 100$", new Vector2(400, 330), Color.Green);
                spriteBatch.DrawString(bFont, "Every worker costs 10$/min", new Vector2(400, 360), Color.Green);
                spriteBatch.DrawString(bFont, "Have fun and Good luck with this amazing game", new Vector2(400, 390), Color.Green);
            }
            if (!playGame)
                spriteBatch.DrawString(dFont, "GAME OVER!", new Vector2(400, 150), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void AddWorker()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && canAddWorker && gameObjects.Count <= 40 && Factory.GoldAmount >= 0 && !isPaused)
            {
                GameObject go = new Worker(new Vector2(rnd.Next(900, 1000), rnd.Next(260, 280)), "man", 0.3f, number.ToString());
                go.LoadContent(Content);
                (go as Worker).WThread = new Thread((go as Worker).Move);
                (go as Worker).WThread.IsBackground = true;
                (go as Worker).WThread.Start();
                gameObjects.Add(go);
                number++;
                //if (Factory.GoldAmount >= 100)
                //Factory.GoldAmount -= 100;
                canAddWorker = false;

            }
            if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
                canAddWorker = true;
            }

        }
        public void DeleteWorker()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F) && canDeleteWorker && number > 0 && !isPaused)
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
            if (Keyboard.GetState().IsKeyDown(Keys.S) && isPaused && number > 0 && playGame)
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
                if (firstStart)
                {
                    firstStart = false;
                    timerThread.Start();
                }
                else timerThread.Resume();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.P) && !isPaused && number > 0)
            {
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                        (go as Worker).WThread.Abort();
                }
                isPaused = true;
                timerThread.Suspend();
            }
        }

        public void UpdateTimer()
        {
            while (true)
            {
                Thread.Sleep(1000);
                sec--;
                if (sec < 0)
                {
                    if ((min + sec) <= 0) playGame = false;
                    min--;
                    sec = 59;
                    if (min < 14)
                        CheckFactoryStatus();
                }
            }
        }

        public void CheckFactoryStatus()
        {
            if (Factory.GoldAmount >= number * 10)
                Factory.GoldAmount -= number * 10;
            else
            {
                Factory.GoldAmount = 0;
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                        (go as Worker).WThread.Abort();
                }
                isPaused = true;
                playGame = false;
            }
        }

        public void RestartGame()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && canRestart && !firstStart)
            {
                if (number > 0)
                {
                    /*
                    foreach (GameObject go in gameObjects)
                    {
                        if (go is Worker)
                        {
                            (go as Worker).WThread.Abort();
                            gameObjects.Remove(go);
                        }
                    }*/
                   while (number > 0)
                    {
                        (gameObjects[gameObjects.Count - 1] as Worker).WThread.Abort();
                        gameObjects.RemoveAt(gameObjects.Count - 1);
                        number--;
                    }
                }
                //number = 0;
                for (int i = 0; i < 5; i++)
                {
                    number++;
                    GameObject go = new Worker(new Vector2(rnd.Next(900, 1000), rnd.Next(260, 280)), "man", 0.3f, number.ToString());
                    go.LoadContent(Content);
                    (go as Worker).WThread = new Thread((go as Worker).Move);
                    (go as Worker).WThread.IsBackground = true;
                    gameObjects.Add(go);
                }
                playGame = true;
                isPaused = true;
                min = 15;
                sec = 0;
                Factory.GoldAmount = 0;
                canRestart = false;
                timerThread.Suspend();
            }
            if (Keyboard.GetState().IsKeyUp(Keys.R))
            {
                canRestart = true;
            }
        }
    }
}