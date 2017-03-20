using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GoldMiningString
{
   abstract class GameObject
    {
        /// <summary>
        /// The GameObject's position
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The GameObject's sprite
        /// </summary>
        protected Texture2D sprite;
        protected string spriteName;
        protected float scale;


        /// <summary>
        /// The GameObject's constructor
        /// </summary>
        public GameObject(Vector2 position, string spriteName, float scale)
        {
            this.position = position;
            this.spriteName = spriteName;
            this.scale = scale;
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spriteName);
        }
        public virtual void update()
        {
            //blank untill we fire something in there
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 1);
        }
    }
}
