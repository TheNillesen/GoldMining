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
    enum Action { WorkRight, WorkLeft, UseWsForward, UseWsBack, UseBankForward, UseBankBack, CanteenForward, CanteenBack };

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
        public Worker(Vector2 position, string spriteName, float scale, string label) : base(position, spriteName, scale)
        {
            this.label = label;
            currentAction = Action.WorkLeft;
            goldAmount = 0;
            speed = GameWorld.Instance.Rnd.Next(50, 80);
            //wThread = new Thread(Move);
            //wThread.IsBackground = true;
            //wThread.Start();
        }

        /// <summary>
        /// Draws the GameObject
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.AFont, goldAmount.ToString()+ "$", new Vector2(position.X + 10, position.Y - 10), Color.DarkRed);
            spriteBatch.DrawString(GameWorld.Instance.AFont, label, new Vector2(position.X, position.Y - 5), Color.Blue);
            // spriteBatch.DrawString(GameWorld.Instance.BFont, speed.ToString(), new Vector2(position.X - 10, position.Y), Color.DarkBlue);
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
                    case Action.WorkLeft:
                        {
                            if (position.X <= 330)
                            {
                                //Thread.Sleep(1000);
                                Mine.GetGold4(this);
                                currentAction = Action.WorkRight;
                            }
                            else if (position.X > 569 && position.X < 570)
                            {
                                currentAction = GameWorld.Instance.Rnd.Next(0, 10) > 1 ? Action.WorkLeft : Action.CanteenForward;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(-1, 0);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    case Action.WorkRight:
                        {
                            if (position.X >= 750)
                            {
                                Factory.ReleaseGold(this);
                                currentAction = GameWorld.Instance.Rnd.Next(0, 10) > 3 ? Action.WorkLeft : Action.UseWsForward;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(1, 0);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    case Action.UseWsForward:
                        {
                            if (position.Y >= 450)
                            {
                                Ws.useWs(this);
                                currentAction = Action.UseWsBack;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(0, 1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    case Action.UseWsBack:
                        {
                            if (position.Y <= GameWorld.Instance.Rnd.Next(250, 270))
                            {
                                currentAction = Action.WorkLeft;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(0, -1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    case Action.CanteenForward:
                        {
                            if (position.Y >= 450)
                            {
                                //Thread.Sleep(1000);
                                Canteen.UseCanteen(this);
                                currentAction = Action.CanteenBack;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(0, 1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    case Action.CanteenBack:
                        {
                            if (position.Y <= GameWorld.Instance.Rnd.Next(250, 270))
                            {
                                currentAction = Action.WorkLeft;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(0, -1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                        //case Action.UseBankForward:
                        //    {
                        //        if (position.Y >= 240)
                        //        {
                        //            //Thread.Sleep(1000);
                        //            Bank.DepositPayment(this);
                        //            currentAction = Action.UseBankBack;
                        //        }
                        //        else
                        //        {
                        //            translation = Vector2.Zero;
                        //            translation += new Vector2(4, 4);
                        //            Thread.Sleep(500 / speed);
                        //        }
                        //    }
                        //    break;
                        //case Action.UseBankBack:
                        //    {
                        //        if (position.Y <= GameWorld.Instance.Rnd.Next(150, 250))
                        //        {
                        //            currentAction = Action.WorkLeft;
                        //        }
                        //        else
                        //        {
                        //            translation = Vector2.Zero;
                        //            translation += new Vector2(-4, -4);
                        //            Thread.Sleep(500 / speed);
                        //        }
                        //    }
                        //    break;
                }
                position += translation * speed / 100;
            }
        }
    }
}
