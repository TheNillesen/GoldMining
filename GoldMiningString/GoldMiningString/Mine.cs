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
    class Mine : GameObject
    {
        static Semaphore sp = new Semaphore(3, 3);
        static Mutex mtx = new Mutex();
        static object thisLock = new object();

        public Mine(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        {

        }


        //semaphore
        public static void GetGold1(Worker w)
        {
            sp.WaitOne();
            Thread.Sleep(500);
            w.Position = new Vector22(w.Position.X + 40, w.Position.Y);
            w.GoldAmount = 20;
            Thread.Sleep(2000);
            w.Position = new Vector2(w.Position.X - 40, w.Position.Y - 30);
            sp.Release();
        }
    }
}
