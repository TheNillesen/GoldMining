using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldMiningString
{
    class Ws : GameObject
    {
        static Mutex mtx = new Mutex();
        static object thisLock = new object();

        public Ws(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        {

        }

        public static void useWs(Worker w)
        {
            mtx.WaitOne();
            Thread.Sleep(500);
            w.Position = new Vector2(w.Position.X, w.Position.Y + 60);
            Thread.Sleep(2000);
            w.Position = new Vector2(w.Position.X + 15, w.Position.Y - 60);
            mtx.ReleaseMutex();
        }
    }
}
