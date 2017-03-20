using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GoldMiningString
{
    enum Action { WorkForward, WorkBack, UseWsForward, UseWsBack };

    class Worker : GameObject
    {
        private string label;
        Vector2 translation;
        Thread wThread;
        int speed;
        int goldAmount;
        Action currentAction;

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public int GoldAmount
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

        public Thread WThread
        {
            get
            {
                return wThread;
            }

            set
            {
                wThread = value;
            }
        }

        /// <summary>
        /// The GameObject's constructor
        /// </summary>

        public Worker(Vector2 position, string spriteName, float scale, string lable) : base(position, spriteName, scale)
        {
            this.label = label;
            currentAction = Action.WorkForward;
            goldAmount = 0;
            speed = GameWorld.Instance.Rnd.Next(40, 100);
            wThread = new Thread(Move);
            wThread.IsBackground = true;
            wThread.Start();

        }

        /// <summary>
        /// Draws the GameObject
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.BFont, label + " $" + goldAmount.ToString(), new Vector2(position.X, position.Y - 10), Color.DarkRed);
            spriteBatch.DrawString(GameWorld.Instance.BFont, speed.ToString(), new Vector2(position.X - 10, position.Y), Color.DarkBlue);
            base.Draw(spriteBatch);
        }

        public void Move()
        {

            //The current translation of the player
            //We are restting it to make sure that he stops moving if not keys are pressed
            while (true)
            {
                switch (currentAction)
                {
                    case Action.WorkForward:
                        {
                            if (position.X >= 820)
                            {
                                //Thread.Sleep(1000);
                                Mine.GetGold1(this);
                                currentAction = Action.WorkBack;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(1, 0);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    case Action.WorkBack:
                        {
                            if (position.X <= 400)
                            {
                                Factory.ReleaseGold(this);
                                currentAction = GameWorld.Instance.Rnd.Next(0, 10) > 3 ? Action.WorkForward : Action.UseWsForward;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(-1, 0);
                                Thread.Sleep(500 / speed);
                            }
                        }
                        break;
                    case Action.UseWsForward:
                        {
                            if (position.Y >= 450)
                            {
                                //Thread.Sleep(1000);
                                Ws.useWs(this);
                                currentAction = Action.UseWsBack;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(0, 1);
                                Thread.Sleep(500 / speed);
                            }
                        }
                        break;
                    case Action.UseWsBack:
                        {
                            if (position.Y <= GameWorld.Instance.Rnd.Next(150, 250))
                            {
                                currentAction = Action.WorkForward;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(0, -1);
                                Thread.Sleep(500 / speed);
                            }
                        }
                        break;
                }
                position += translation * speed / 100;
            }
        }

    }
}
