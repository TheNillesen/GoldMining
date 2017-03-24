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
    // Possible Worker's actions in the Game
    enum Action { WorkRight, WorkLeft, UseWcForward, UseWcBack, UseBankForward, UseBankBack, CanteenForward, CanteenBack };

    /// <summary>
    /// Represents the Worker
    /// </summary>
    class Worker : GameObject
    {
        private string label; // Worker's current number
        Vector2 translation; // Worker's translation
        Thread wThread; // Worker's thread
        int speed; // Worker's speed
        int goldAmount; // Current gold amount
        Action currentAction; // Worker's current action

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

        public int Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
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
            speed = 0;
            //speed = GameWorld.Instance.Rnd.Next(50, 80);
            wThread = new Thread(Move); // Initializes the thread, which uses the method Move
            wThread.IsBackground = true; // Sets thrad as background
            wThread.Start(); // Starts thread
        }

        /// <summary>
        /// Draws two Ores more
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.AFont, goldAmount.ToString()+ "$", new Vector2(position.X + 10, position.Y - 10), Color.DarkRed);
            spriteBatch.DrawString(GameWorld.Instance.AFont, label, new Vector2(position.X, position.Y - 5), Color.Blue);
            // spriteBatch.DrawString(GameWorld.Instance.BFont, speed.ToString(), new Vector2(position.X - 10, position.Y), Color.DarkBlue);
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Worker's funktionaity
        /// </summary>
        public void Move()
        {

            //The current translation of the player
            //We are restting it to make sure that he stops moving if not keys are pressed
            while (true)
            {
                switch (currentAction)
                {
                    // Worker's motion on direction from the Factory to the Mine
                    case Action.WorkLeft:
                        {
                            if (position.X <= 330)
                            {
                                // Cheks how many ores can Worker access (from 1 to 3)
                                int selectedOre = GameWorld.Instance.Rnd.Next(1, GameWorld.Instance.OreAmounts + 1);
                                if (selectedOre ==1)
                                    Mine.GetGold1(this); // has to be performed if first ore is chosen
                                else if (selectedOre == 2)
                                    Mine.GetGold2(this); // has to be performed if second ore is chosen
                                else if (selectedOre == 3)
                                    Mine.GetGold3(this); // has to be performed if third ore is chosen
                                currentAction = Action.WorkRight;
                            }
                            else if (position.X > 569 && position.X < 570)
                            {
                                // Worker chooses direction: work or canteen
                                currentAction = GameWorld.Instance.Rnd.Next(0, 10) > 1 ? Action.WorkLeft : Action.CanteenForward;
                            }
                            else
                            {
                                // Continues motion 
                                translation = Vector2.Zero;
                                translation += new Vector2(-1, 0);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                        // Worker's motion on direction from the Mine to the Factory
                    case Action.WorkRight:
                        {
                            if (position.X >= 750)
                            {
                                Factory.ReleaseGold(this); // has to be performed when Worker comes to the Factory
                                // Worker chooses direction: work or Ws
                                currentAction = GameWorld.Instance.Rnd.Next(0, 10) > 3 ? Action.WorkLeft : Action.UseWcForward;
                            }
                            else
                            {
                                // Continues motion
                                translation = Vector2.Zero;
                                translation += new Vector2(1, 0);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    // Worker's motion on direction from the Factory to Ws
                    case Action.UseWcForward:
                        {
                            if (position.Y >= 460)
                            {
                                Wc.useWc(this); // has to be performed when Worker comes to the Ws
                                currentAction = Action.UseWcBack; // after Ws Worker has to move back
                            }
                            else
                            {
                                // Continues motion
                                translation = Vector2.Zero;
                                translation += new Vector2(0, 1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    // Worker's motion on direction from the Ws
                    case Action.UseWcBack:
                        {
                            // After coming up from the Ws Worker has to continue to work
                            if (position.Y <= GameWorld.Instance.Rnd.Next(250, 270))
                            {
                                currentAction = Action.WorkLeft;
                            }
                            else
                            {
                                // Continues motion
                                translation = Vector2.Zero;
                                translation += new Vector2(0, -1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    // Worker's motion on direction to the Canteen
                    case Action.CanteenForward:
                        {
                            if (position.Y >= 450)
                            {
                                Canteen.UseCanteen(this); // Has to be performed when Worker comes to the Canteen
                                currentAction = Action.CanteenBack; // After the Canteen Worker has to move back
                            }
                            else
                            {
                                // Continues motion
                                translation = Vector2.Zero;
                                translation += new Vector2(0, 1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                    // Worker's motion on direction from the Canteen
                    case Action.CanteenBack:
                        {
                            // After coming up from the Canteen Worker has to continue to work
                            if (position.Y <= GameWorld.Instance.Rnd.Next(250, 270))
                            {
                                currentAction = Action.WorkLeft;
                            }
                            else
                            {
                                // Continues motion
                                translation = Vector2.Zero;
                                translation += new Vector2(0, -1);
                                Thread.Sleep(10);
                            }
                        }
                        break;
                        /*
                    case Action.UseBankForward:
                        {
                            if (position.Y >= 240)
                            {
                                //Thread.Sleep(1000);
                                Bank.DepositPayment(this);
                                currentAction = Action.UseBankBack;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(4, 4);
                                Thread.Sleep(500 / speed);
                            }
                        }
                        break;
                    case Action.UseBankBack:
                        {
                            if (position.Y <= GameWorld.Instance.Rnd.Next(150, 250))
                            {
                                currentAction = Action.WorkLeft;
                            }
                            else
                            {
                                translation = Vector2.Zero;
                                translation += new Vector2(-4, -4);
                                Thread.Sleep(500 / speed);
                            }
                        }
                        break;*/
                }
                position += translation * speed / 100;
            }
        }
    }
}
