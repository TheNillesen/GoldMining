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
    /// <summary>
    /// Represents the GameObject
    /// </summary>
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

        /// <summary>
        /// The GamObject's spritename
        /// </summary>
        protected string spriteName;

        /// <summary>
        /// The GameObject' scale
        /// </summary>
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

        /// <summary>
        /// Loads the GameObject's content, this is where we load sounds, sprites etc.
        /// </summary>
        /// <param name="content">The Content form the GameWorld</param>
        public virtual void LoadContent(ContentManager content)
        {
            //Loads the sprite variable
            sprite = content.Load<Texture2D>(spriteName);
        }

        /// <summary>
        /// Draws the GameObject
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draws the GameObject by using the sprite and the position
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 1);
            // spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
