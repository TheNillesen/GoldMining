using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldMiningString
{
    /// <summary>
    /// Represents the Ws
    /// </summary>
    class Wc : GameObject
    {
        static Mutex mtx = new Mutex();
        //static object thisLock = new object();

        /// <summary>
        /// The Ws's constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="spriteName"></param>
        /// <param name="scale"></param>
        public Wc(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        { }

        /// <summary>
        /// Functionlity, which has to be performd by entered Worker's threads
        /// </summary>
        /// <param name="w"></param>
        public static void useWs(Worker w)
        {
            try
            {
                while (w.Speed == 0) { } // Suspends Worker's motion if the game is paused
                mtx.WaitOne(); // Only one Worker's thread
                Thread.Sleep(500); // Thread sleeps 0.5 second
                w.Position = new Vector2(w.Position.X, 510); // Places the Worker inside into the Ws
                Thread.Sleep(2000); // Thread sleeps 2 seconds
                w.Position = new Vector2(w.Position.X - 15, w.Position.Y - 60); // Places the Worker outside the Ws
                //mtx.ReleaseMutex();
            }
            catch (Exception)
            {

            }
            finally
            {
                mtx.ReleaseMutex(); // Releases the mutex
            }

            /*
           Monitor.Enter(thisLock);
            try
            {
                Thread.Sleep(500);
                w.Position = new Vector2(w.Position.X, 510);
                Thread.Sleep(2000);
                w.Position = new Vector2(w.Position.X - 15, w.Position.Y - 60);
            }
            finally
            {
                Monitor.Exit(thisLock);
            }*/
        }


    }
}
