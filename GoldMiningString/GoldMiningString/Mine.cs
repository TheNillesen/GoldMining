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
        { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Vector2(200, 150), null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 1);
            spriteBatch.Draw(sprite, new Vector2(200, 330), null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

        // Semaphore
        public static void GetGold1(Worker w)
        {
            sp.WaitOne();
            Thread.Sleep(500);
            w.Position = new Vector2(w.Position.X - 40, w.Position.Y);
            w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30);
            Thread.Sleep(2000);
            w.Position = new Vector2(w.Position.X + 40, w.Position.Y - 30);
            sp.Release();
        }
        // Lock
        public static void GetGold2(Worker w)
        {
            lock (thisLock)
            {
                Thread.Sleep(500);
                w.Position = new Vector2(270, w.Position.Y);
                w.GoldAmount = GameWorld.Instance.Rnd.Next(10, 30);
                Thread.Sleep(2000);
                w.Position = new Vector2(w.Position.X + 60, w.Position.Y - 30);
            }
        }
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
        }
    }
}
