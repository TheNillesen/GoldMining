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
    class Factory : GameObject
    {
        /// <summary>
        /// The object that is going to be locked
        /// </summary>
        static object thisLock = new object();

        /// <summary>
        /// The object that is going to be locked
        /// </summary>
        static object thisLock2 = new object();

        /// <summary>
        /// The Factory's balance
        /// </summary>
        static int goldAmount;

        //static Mutex mtx = new Mutex();

        public static int GoldAmount
        {
            get { lock(thisLock2) { return goldAmount; } }
            set { lock(thisLock2) { goldAmount = value; } }
        }

        /// <summary>
        /// The Factory's constuctor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="spriteName"></param>
        /// <param name="scale"></param>
        public Factory(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        {
            goldAmount = 0; // Sets Factory's balance to 0
        }

        /// <summary>
        /// Functionlity, which has to be performd by entered Worker's threads
        /// </summary>
        /// <param name="w"></param>
        public static void ReleaseGold(Worker w)
        {
            Monitor.Enter(thisLock); // Monitor enter
            try
            {
                while (w.Speed == 0) { } // Suspends Worker's motion if the game is paused
                Thread.Sleep(500); // Thread sleeps 0.5 second
                w.Position = new Vector2(770, 200); // Places the Worker inside into the Factory
                Thread.Sleep(2000); // Thread sleeps 2 seconds
                goldAmount += w.GoldAmount; // Increases Factory's balance by current Worker's gold amount
                w.GoldAmount = 0; // Sets current Worker gold amount to 0
                w.Position = new Vector2(750, GameWorld.Instance.Rnd.Next(260, 280)); // Places the Worker outside the Factory
            }
            finally
            {
                Monitor.Exit(thisLock); // Monitor exit
            }

            /*
            try
            {
                mtx.WaitOne();
                Thread.Sleep(500);
                w.Position = new Vector2(770, 200);
                Thread.Sleep(2000);
                goldAmount += w.GoldAmount;
                w.GoldAmount = 0;
                w.Position = new Vector2(750, GameWorld.Instance.Rnd.Next(260, 280));
                //mtx.ReleaseMutex();
            }
            catch (Exception)
            {
                //mtx.Dispose();
            }
            finally
            {
                mtx.ReleaseMutex();
            }*/


            /*
            lock (thisLock)
            {
                Thread.Sleep(500);

               // w.Position = new Vector2(w.Position.X + 40, w.Position.Y);
                w.Position = new Vector2(770, 200);
                Thread.Sleep(1000);
                goldAmount += w.GoldAmount;
                w.GoldAmount = 0;
                w.Position = new Vector2(740, GameWorld.Instance.Rnd.Next(240, 270));
            }*/
        }
        /// <summary>
        /// Draws the Factory's current balance
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.BFont, "Balance,$: " + goldAmount.ToString(), new Vector2(position.X - 60, position.Y - 15), Color.DarkBlue);
            base.Draw(spriteBatch);
        }
    }
}

