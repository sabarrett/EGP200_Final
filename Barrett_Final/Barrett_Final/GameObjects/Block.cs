using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SciFiDemo.Rendering;

namespace SciFiDemo
{
    class Block : Drawable
    {
        protected Sprite sprite;

        protected Rectangle hitbox;

        protected Vector2 position;

        public Block()
        {
            sprite = null;
            hitbox = new Rectangle(0, 0, 128, 64);
        }

        public virtual void LoadContent(ContentManager content)
        {
            Texture2D texture = content.Load<Texture2D>("blocks");
            Rectangle sourceRect = new Rectangle(0, 0, 128, 64);

            sprite = new Sprite(texture, sourceRect);
            sprite.mPosition = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            sprite.mPosition = position;
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
        }
    }
}
