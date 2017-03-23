using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldMiningString
{
    /// <summary>
    /// Represents the Mine
    /// </summary>
    class Mine : GameObject
    {
        //static Semaphore sp = new Semaphore(3, 3);
        //static Mutex mtx = new Mutex();

        /// <summary>
        /// The Object that is going to be locked
        /// </summary>
        static object thisLock1 = new object();

        /// <summary>
        /// The Object that is going to be locked
        /// </summary>
        static object thisLock2 = new object();

        /// <summary>
        /// The Object that is going to be locked
        /// </summary>
        static object thisLock3 = new object();

        /// <summary>
        /// The Mine's constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="spriteName"></param>
        /// <param name="scale"></param>
        public Mine(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        { }

        /// <summary>
        /// Draws two more Ores
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Vector2(200, 150), null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 1);
            spriteBatch.Draw(sprite, new Vector2(200, 330), null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

        // Semaphore
        /*
        public static void GetGold1(Worker w)
        {
            sp.WaitOne();
            Thread.Sleep(500);
            w.Position = new Vector2(w.Position.X - 40, w.Position.Y);
            w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30);
            Thread.Sleep(2000);
            w.Position = new Vector2(w.Position.X + 40, w.Position.Y - 30);
            sp.Release();
        }*/

        /// <summary>
        /// Functionlity, which has to be performd by entered Worker's threads, when first Ore is chosen
        /// </summary>
        /// <param name="w"></param>
        public static void GetGold1(Worker w)
        {
            lock (thisLock1) // locks an object
            {
                while(w.Speed == 0) { } // Suspends Worker's motion if the game is paused
                Thread.Sleep(500); // Thread sleeps 0.5 second
                w.Position = new Vector2(270, 260); // Places the Worker inside into the Mine
                Thread.Sleep(4000); // Thread sleeps 2 seconds
                w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30); // The current Worker gets gold (from 10 to 29)
                w.Position = new Vector2(w.Position.X + 60, GameWorld.Instance.Rnd.Next(230,250)); // Places the Worker outside the Mine
            }
        }

        /// <summary>
        /// Functionlity, which has to be performd by entered Worker's threads, when second Ore is chosen
        /// </summary>
        /// <param name="w"></param>
        public static void GetGold2(Worker w)
        {
            lock (thisLock2) // locks an object
            {
                while (w.Speed == 0) { } // Suspends Worker's motion if the game is paused
                Thread.Sleep(500); // Thread sleeps 0.5 second
                w.Position = new Vector2(270, 170); // Places the Worker inside into the Mine
                Thread.Sleep(4000); // Thread sleeps 2 seconds
                w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30); // The current Worker gets gold (from 10 to 29)
                w.Position = new Vector2(w.Position.X + 60, GameWorld.Instance.Rnd.Next(230, 250)); // Places the Worker outside the Mine
            }
        }

        /// <summary>
        /// Functionlity, which has to be performd by entered Worker's threads, when third Ore is chosen
        /// </summary>
        /// <param name="w"></param>
        public static void GetGold3(Worker w)
        {
            lock (thisLock3) // locks an object
            {
                while (w.Speed == 0) { } // Suspends Worker's motion if the game is paused
                Thread.Sleep(500); // Thread sleeps 0.5 second
                w.Position = new Vector2(270, 350); // Places the Worker inside into the Mine
                Thread.Sleep(4000); // Thread sleeps 2 seconds
                w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30); // The current Worker gets gold (from 10 to 29)    
                w.Position = new Vector2(w.Position.X + 60, GameWorld.Instance.Rnd.Next(230, 250)); // Places the Worker outside the Mine
            }
        }
        /*
        //Monitor
        public static void GetGold3(Worker w)
        {
            Monitor.Enter(thisLock);
            try
            {
                Thread.Sleep(500);
                w.GoldAmount = 20;
                w.Position = new Vector2(w.Position.X + 40, w.Position.Y);
                Thread.Sleep(1000);
            }
            finally
            {
                Monitor.Exit(thisLock);
            }
        }
        // Mutex
        public static void GetGold4(Worker w)
        {
            mtx.WaitOne();
            Thread.Sleep(500);
            w.Position = new Vector2(w.Position.X - 60, w.Position.Y);
            w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30);
            Thread.Sleep(2000);
            w.Position = new Vector2(w.Position.X + 60, w.Position.Y - 30);
            mtx.ReleaseMutex();
        }*/
    }
}
