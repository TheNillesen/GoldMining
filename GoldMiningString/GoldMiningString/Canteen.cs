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

        public static void UseCanteen(Worker w)
        {
            try
            {
                sp.WaitOne();
                Thread.Sleep(500);
                w.Position = new Vector2(GameWorld.Instance.Rnd.Next(540, 570), GameWorld.Instance.Rnd.Next(500, 550));
                Thread.Sleep(4000);
                w.Position = new Vector2(530, 450);
                sp.Release();
            }
            catch (Exception)
            {
                sp.Release();
            }
        }
    }
}
