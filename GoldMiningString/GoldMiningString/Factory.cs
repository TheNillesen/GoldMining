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
        static object thisLock = new object();
        static int goldAmount;

        public Factory(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        {
            goldAmount = 0;
        }

        public static void ReleaseGold(Worker w)
        {
            lock (thisLock)
            {
                Thread.Sleep(500);
                w.GoldAmount = 0;
                w.Position = new Vector2(w.Position.X - 40, w.Position.Y);
                Thread.Sleep(1000);
                goldAmount += 20;
                w.Position = new Vector2(w.Position.X + 40, w.Position.Y + 30);
            }
        }

            public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.BFont, "$ " + goldAmount.ToString(), new Vector2 (position.X, position.Y + 5), Color.DarkBlue);
            base.Draw(spriteBatch);
        }
    }

}

