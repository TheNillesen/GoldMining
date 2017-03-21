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
    class Bank : GameObject
    {
        static object thisLock = new object();
        static int goldAmount;

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

        public Bank(Vector2 position, string spriteName, float scale) : base(position, spriteName, scale)
        {
            goldAmount = 0;
        }

        public static void DepositPayment(Worker w)
        {
            lock (thisLock)
            {
                Thread.Sleep(500);

                //denne linje forstår jeg ikke
                w.Position = new Vector2(w.Position.X + 40, w.Position.Y);

                //dette er hvor worker bliver spawnet når de kommer til banken
                w.Position = new Vector2(250, 450);

                //hvor længe en worker venter i banken for at indsætte penge
                Thread.Sleep(1000);

                //worker aflevere guld
                goldAmount += w.GoldAmount;

                //workers guld er nu 0
                w.GoldAmount = 0;

                //worker spawnes efter at ahve tømt penge
                w.Position = new Vector2(180, GameWorld.Instance.Rnd.Next(350, 380));
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.BFont, "$ " + goldAmount.ToString(), new Vector2(position.X, position.Y + 5), Color.DarkBlue);
            base.Draw(spriteBatch);
        }
    }
}
    