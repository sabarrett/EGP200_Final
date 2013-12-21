using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SciFiDemo.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SciFiDemo.GameObjects
{
    class Spaceship : Drawable
    {
        static Texture2D sTexture;

        Sprite mSprite;

        Vector2 mPos;
        Vector2 mVel;

        public Spaceship()
        {
            mSprite = new Sprite();
            mPos = Vector2.Zero;
            mVel = Vector2.Zero;
        }

        public static void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("Images/spaceship");
        }

        public void Update(GameTime gameTime)
        {
            mSprite.Create(sTexture);
            mPos += mVel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mSprite.mPosition = mPos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch);
        }

        public void SetPosition(Vector2 position)
        {
            mPos = position;
            mSprite.mPosition = mPos;
        }
    }
}
