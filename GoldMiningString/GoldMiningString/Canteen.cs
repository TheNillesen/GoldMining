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
    ///  Represents Canteen
    /// </summary>
    class Canteen : GameObject
    {
        // Create a new mutex (initial count 3, max capacity 4)
        static Semaphore sp = new Semaphore(3, 4);

        /// <summary>
        /// The Canteen's constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="spriteName"></param>
        /// <param name="scale"></param>
        public Canteen(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        public static void UseCanteen(Worker w)
        {
            try
            {
                while (w.Speed == 0) { } // Suspends Worker's motion if the game is paused
                sp.WaitOne();
                Thread.Sleep(500); // Thread sleeps 0.5 second
                w.Position = new Vector2(GameWorld.Instance.Rnd.Next(540, 570), GameWorld.Instance.Rnd.Next(500, 550)); // Places the Worker inside into the Ws
                Thread.Sleep(4000); // Thread sleeps 4 seconds
                w.Position = new Vector2(530, 450); // Places the Worker outside the Ws

                                                    
            }
            catch (Exception)
            {
               
            }
            finally
            {
                sp.Release(); // Releases the semaphore
            }
        }
    }
}
