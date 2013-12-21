using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Rendering;

namespace SciFiDemo.GameObjects
{
    enum Directions
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    class Missile : Drawable
    {
        static Texture2D sTexture;

        Sprite mSprite;

        Vector2 mPos;
        Vector2 mVel;

        const float SPEED = 1500f; // pixels per second

        public Missile()
        {
            mSprite = new Sprite();
            mPos = Vector2.Zero;
            mVel = new Vector2(SPEED, 0);
        }

        public static void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("rocket-128x41");
        }

        public void Update(GameTime gameTime)
        {
            mSprite.Create(sTexture, new Rectangle(0, 0, 128, 41));
            mPos += mVel * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.mPosition = mPos;
            mSprite.Draw(spriteBatch);
        }

        public void SetVelocity(Directions direction)
        {
            switch (direction)
            {
                case Directions.UP:
                    {
                        mVel = new Vector2(0, -SPEED);
                        mSprite.rotation = (float)Math.PI * 2f * 0.75f;
                    }
                    break;
                case Directions.DOWN:
                    {
                        mVel = new Vector2(0, SPEED);
                        mSprite.rotation = (float)Math.PI * 2f * 0.25f;
                    }
                    break;
                case Directions.LEFT:
                    {
                        mVel = new Vector2(-SPEED, 0);
                        mSprite.effect = SpriteEffects.FlipHorizontally;
                    }
                    break;
                case Directions.RIGHT:
                    {
                        mVel = new Vector2(SPEED, 0);
                    }
                    break;
            }
        }

        public void SetPosition(Vector2 position)
        {
            mPos = position;
        }
    }
}
