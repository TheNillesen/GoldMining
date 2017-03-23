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
    /// Represents the GAmeWorld
    /// </summary>
    public class GameWorld : Game
    {
        Thread timerThread; // Game timer's thread
        GraphicsDeviceManager graphics; // GraphicsDeviceManager's instace
        SpriteBatch spriteBatch; // SpriteBatch's instance
        private static GameWorld instance; // GameWorld's instance (pattern singletone)
        private List<GameObject> gameObjects; // The list of gameobjects
        SpriteFont bFont; // Font 12
        SpriteFont aFont; // Font 8
        SpriteFont dFont; // Font 20
        int number; // current workers amount and number for last worker
        Random rnd; // Random variable
        bool canAddWorker; // Used for adding only one worker
        bool canDeleteWorker; // Used for deleting only one worker 
        bool canByOre; // Used to by only one ore
        bool canRestart; // Used for restarting game
        bool isPaused; // Used to suspend game
        bool firstStart; // Used when game started first time
        float min, sec; // Game timer's minutes and seconds
        bool playGame;  // Used when game is running
        Song backGroundSound;
        int oresAmount; // The ores amount
        Texture2D bannedSprite; // The sprite "banned"

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
        public int OreAmounts
        {
            get
            {
                return oresAmount;
            }
        }

        /// <summary>
        ///  The GameWorld's constructor
        /// </summary>
        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000; // Form's width
            graphics.PreferredBackBufferHeight = 650; // Form's hight
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
            isPaused = true; // Makes sure that game dosn't run after start
            min = 15; // Sets game timer's minutes to 15
            sec = 0; // Sets game timer's seconds to 15
            number = 0; // Sets workers amount to 0
            canAddWorker = true;
            canDeleteWorker = true;
            canByOre = true;
            canRestart = true;
            playGame = true;
            oresAmount = 3; // Ore's amount
            timerThread = new Thread(UpdateTimer); // Initializes game timer thread, which uses the UpdateTimer method
            timerThread.IsBackground = true; // Sets game timer thread as background
            gameObjects = new List<GameObject>();
            gameObjects.Add(new Mine(new Vector2(200, 240), "ore", 0.6f)); // Creats the mine's instance and adds to the gameobject's list
            gameObjects.Add(new Factory(new Vector2(670, 50), "factory", 0.7f)); // Creats the factory's instance and adds to the gameobject's list
            gameObjects.Add(new Wc(new Vector2(720, 500), "ws", 0.2f)); // Creats the ws's instance and adds to the gameobject's list
            gameObjects.Add(new Canteen(new Vector2(450, 480), "canteen2", 0.5f)); // Creats the cantineen's instance and adds to the gameobject's list
            // gameObjects.Add(new Bank(new Vector2(200, 400), "bank", 0.9f));
            backGroundSound = Content.Load<Song>("Jazz");
            MediaPlayer.Play(backGroundSound);
            MediaPlayer.IsRepeating = true;
   
            // Creates 5 Worker objects and adds them to the Gameobject's list
            for (int i = 0; i < 5; i++)
            {
                number++; // increments Worker object's amount
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
            bannedSprite= Content.Load<Texture2D>("banned");
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
            //deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (playGame)
            {
                //if (!isPaused)
                  //  UpdateTimer();
                AddWorker(); // This method checks if necessary to add one Worker's object
                ByOre(); // This method checks if necessary to add Ore's Worker object
                DeleteWorker(); // This method checks if necessary to delete one Worker's object
                StartStop(); // This method checks if necessary to suspend or resume game
            }
            RestartGame(); // This method checks if necessary to restart game

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
            // Draws all gameobjects
            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);
            }
            spriteBatch.DrawString(bFont, "Workers: " + number, new Vector2(10, 100), Color.Black); // draws Worker object's amount
            spriteBatch.DrawString(bFont, "[A] - to recruit worker", new Vector2(10, 200), Color.Black);
            spriteBatch.DrawString(bFont, "[F] - to fire worker", new Vector2(10, 230), Color.Black);
            spriteBatch.DrawString(bFont, "[O] - to by ore", new Vector2(10, 260), Color.Black);
            spriteBatch.DrawString(bFont, "[P] - pause", new Vector2(10, 450), Color.Black);
            spriteBatch.DrawString(bFont, "[S] - start / resume game", new Vector2(10, 480), Color.Black);
            spriteBatch.DrawString(bFont, "[esc] - exit game", new Vector2(10, 580), Color.Black);
            spriteBatch.DrawString(bFont, "[R] - restart game", new Vector2(10, 550), Color.Black);
            spriteBatch.DrawString(dFont, string.Format("Time left: {0:00}:{1:00}", min, sec), new Vector2(10, 30), Color.Red);
            if (firstStart)
            {
                spriteBatch.DrawString(bFont, "Buy workers and take care about the factory", new Vector2(400, 300), Color.Green);
                spriteBatch.DrawString(bFont, "To buy a worker costs 100$", new Vector2(400, 330), Color.Green);
                spriteBatch.DrawString(bFont, "Every worker costs 10$/min", new Vector2(400, 360), Color.Green);
                spriteBatch.DrawString(bFont, "Have fun and Good luck with this amazing game", new Vector2(400, 390), Color.Green);
            }
            if (!playGame && canRestart)
                spriteBatch.DrawString(dFont, "GAME OVER!", new Vector2(400, 150), Color.Red);
            if (oresAmount < 2)
            spriteBatch.Draw(bannedSprite, new Vector2(205, 140), null, Color.White, 0f, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            if (oresAmount < 3)
            spriteBatch.Draw(bannedSprite, new Vector2(205, 320), null, Color.White, 0f, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            //if (!canRestart)
                //spriteBatch.DrawString(dFont, "WATE A MOMENT", new Vector2(400, 150), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// This method checks if necessary to add one Worker object
        /// </summary>
        public void AddWorker()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && canAddWorker && gameObjects.Count <= 40 && Factory.GoldAmount >= 0 && !isPaused)
            {
                number++;  // increments Worker object's amount
                // Creates one Worker object and adds them to the Gameobject's list
                GameObject go = new Worker(new Vector2(rnd.Next(900, 1000), rnd.Next(260, 280)), "man", 0.3f, number.ToString());
                go.LoadContent(Content);
                //(go as Worker).WThread = new Thread((go as Worker).Move);
                //(go as Worker).WThread.IsBackground = true;
                //(go as Worker).WThread.Start();
                (go as Worker).Speed = rnd.Next(50, 80); // Sets  speed to 50-79
                gameObjects.Add(go); // Adds Worker object to the GameObject's list

                //if (Factory.GoldAmount >= 100) // Checks if enough funds to by Worker
                //Factory.GoldAmount -= 100; // Decrements Factory's balance by 100
                canAddWorker = false; // Makes possible to add only one Worker
            }
            // Makes possible to add next Worker
            if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
                canAddWorker = true;
            }
        }

        /// <summary>
        /// This method checks if necessary to add(give access) one Ore
        /// </summary>
        public void ByOre()
        {
            // Checs if enough funds on Factory's balance
            if (Keyboard.GetState().IsKeyDown(Keys.O) && canByOre && Factory.GoldAmount >= 500 && !isPaused)
            {
                Factory.GoldAmount -= 500; // Decrements Factory's balance by 500
                canByOre = false; // Makes possible to add only one Ore
            }
            // Makes possible to add next Worker
            if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
                canByOre = true;
            }

        }

        /// <summary>
        ///  This method checks if necessary to delete one Worker object
        /// </summary>
        public void DeleteWorker()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F) && canDeleteWorker && number > 0 && !isPaused)
            {
                (gameObjects[gameObjects.Count - 1] as Worker).WThread.Abort(); // Aborts last Worker's thread
                gameObjects.RemoveAt(gameObjects.Count - 1); // Deletes last Worker from the GameObject's list
                number--; // derements Worker object's amount
                canDeleteWorker = false; // Makes possible to delete only one Worker
            }
            // Makes possible to delete next Worker
            if (Keyboard.GetState().IsKeyUp(Keys.F))
            {
                canDeleteWorker = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartStop()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S) && isPaused && number > 0 && playGame)
            {      
                isPaused = false; // If game is not paused
                // Sets all Worker's speed to 50-79
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                    {
                        (go as Worker).Speed = rnd.Next(50, 80);
                    }
                }
                if (firstStart)
                {
                    /*
                    foreach (GameObject go in gameObjects)
                    {
                        if (go is Worker)
                        {
                            (go as Worker).WThread = new Thread((go as Worker).Move);
                            (go as Worker).WThread.IsBackground = true;
                            (go as Worker).WThread.Start();
                        }
                    }*/
                    firstStart = false;
                    timerThread.Start(); // Starts game timer if it is first start
                }
                else timerThread.Resume(); // Resumes game timer if it was suspended
                canRestart = true; // Makes possible to restart game
                MediaPlayer.Resume(); 
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.P) && !isPaused && number > 0)
            {
                /*
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                    {
                        (go as Worker).WThread.Abort();
                        (go as Worker).WThread.Join();
                    }
                }*/
                // Sets all Worker's speed to 50-79
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                    {
                        (go as Worker).Speed = 0;
                    }
                }
               isPaused = true;
                timerThread.Suspend(); // Suspends game timer
                MediaPlayer.Pause();
            }
        }

        /// <summary>
        /// Updates game timer
        /// </summary>
        public void UpdateTimer()
        {
            while (true)
            {
                Thread.Sleep(1000);
                sec--; // Decrements seconds by one
                if (sec < 0)
                {
                    if ((min + sec) <= 0) playGame = false; // Game over if time is expired
                    min--; // Decrements minutes by one
                    sec = 59; // Sets seconds to 59
                    if (min < 14)
                        CheckFactoryStatus(); // Checks if enough funds on the Factory's balance to pay for Workers
                }
            }
        }

        /// <summary>
        /// Checks if enough funds on the Factory's balance to pay for Workers
        /// </summary>
        public void CheckFactoryStatus()
        {
            // If enough funds, Factory's balance decrementes by workers amount * 10
            if (Factory.GoldAmount >= number * 10)
                Factory.GoldAmount -= number * 10;
            else
            {
                Factory.GoldAmount = 0; // Sets Factory's balance to 0
                // Sets all Worker's speed to 0
                foreach (GameObject go in gameObjects)
                {
                    if (go is Worker)
                    {
                        (go as Worker).Speed = 0;
                    }
                }
                isPaused = true;
                playGame = false; // Game over
            }
        }

        /// <summary>
        /// This method checks if necessary to suspend or resume game
        /// </summary>
        public void RestartGame()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && canRestart && !firstStart)
            {
                canRestart = false; // Makes possible to restart only one time
                if (number > 0) // Checks if there are Workers in the GameObjects list
                {
                    /*
                    foreach (GameObject go in gameObjects)
                    {
                        if (go is Worker)
                        {
                            (go as Worker).Speed = 0;
                        }
                    }*/
                    // Aborts Worker's threads and remove Workers from the GameObjects list
                    while (number > 0)
                    {
                        (gameObjects[gameObjects.Count - 1] as Worker).WThread.Abort();
                        //(gameObjects[gameObjects.Count - 1] as Worker).WThread.Join();
                        gameObjects.RemoveAt(gameObjects.Count - 1);
                        number--;
                    }
                }
                // Creates 5 new Worker objects and adds them to the GameObjects list
                for (int i = 0; i < 5; i++)
                {
                    number++;
                    GameObject go = new Worker(new Vector2(rnd.Next(900, 1000), rnd.Next(260, 280)), "man", 0.3f, number.ToString());
                    go.LoadContent(Content);
                    //(go as Worker).WThread = new Thread((go as Worker).Move);
                    //(go as Worker).WThread.IsBackground = true;
                    gameObjects.Add(go);
                }
                playGame = true;
                isPaused = true;
                min = 15; // Sets minutes to 15
                sec = 0; // Sets seconds to 0
                Factory.GoldAmount = 0; // Sets Factory's balance to 0
                
                timerThread.Suspend(); // Suspends Game timer
                oresAmount = 1; // Gives access only to one Ore
            }
        }
    }
}