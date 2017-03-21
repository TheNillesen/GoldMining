using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldMiningString
{
    class Canteen : GameObject
    {
        static Semaphore sp = new Semaphore(3, 3);

        public Canteen(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        { }

        public static void GetGold1(Worker w)
        {
            sp.WaitOne();
            Thread.Sleep(500);
            w.Position = new Vector2(w.Position.X, w.Position.Y + 40);
            w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30);
            Thread.Sleep(2000);
            w.Position = new Vector2(w.Position.X + 40, w.Position.Y - 30);
            sp.Release();
        }
    }
}
