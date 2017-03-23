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
        static object thisLock = new object(); // Object that is going to be locked
        static int goldAmount;
        static Mutex mtx = new Mutex();

        public static int GoldAmount
        {
            get
            {
                return goldAmount;
            }
            set
            {
                goldAmount = value;
            }
        }

        public Factory(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        {
            goldAmount = 0;
        }

        public static void ReleaseGold(Worker w)
        {
            Monitor.Enter(thisLock);
            try
            {
                while (w.Speed == 0)
                { }
                Thread.Sleep(500);
                w.Position = new Vector2(770, 200);
                Thread.Sleep(2000);
                goldAmount += w.GoldAmount;
                w.GoldAmount = 0;
                w.Position = new Vector2(750, GameWorld.Instance.Rnd.Next(260, 280));
            }
            finally
            {
                Monitor.Exit(thisLock);
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
        /// Draws the GameObject
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.BFont, "Balance,$: " + goldAmount.ToString(), new Vector2(position.X - 60, position.Y - 15), Color.DarkBlue);
            base.Draw(spriteBatch);
        }
    }
}

