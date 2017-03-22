using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldMiningString
{
    class Wc : GameObject
    {
        static Mutex mtx = new Mutex();
        //static object thisLock = new object();

        public Wc(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        { }

        public static void useWs(Worker w)
        {
            try
            {
                mtx.WaitOne();
                Thread.Sleep(500);
                w.Position = new Vector2(w.Position.X, 510);
                Thread.Sleep(2000);
                w.Position = new Vector2(w.Position.X - 15, w.Position.Y - 60);
                //mtx.ReleaseMutex();
            }
            catch (Exception)
            {
            }
            finally
            {
                mtx.ReleaseMutex();
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
