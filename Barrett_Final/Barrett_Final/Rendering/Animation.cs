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
    enum DrawOrigins
    {
        ZERO,
        CENTER,
        BASE
    }

    class Animation : Drawable
    {
        List<Sprite> sprites;
        public Vector2 position;

        public DrawOrigins drawOrigin;

        Sprite currentSprite;

        int numFrames;
        int currentFrame;
        int msPerFrame;
        int msUntilNextFrame;


        public Animation()
        {
            sprites = new List<Sprite>();
            position = Vector2.Zero;
            currentSprite = null;
            numFrames = 0;
            currentFrame = 0;
            msPerFrame = 50;
            msUntilNextFrame = msPerFrame;
        }

        public void AddSprite(Sprite sprite)
        {
            sprites.Add(sprite);
            sprite.mPosition = position;
            numFrames++;
        }

        public void Update(GameTime gameTime)
        {
            msUntilNextFrame -= gameTime.ElapsedGameTime.Milliseconds;

            if (msUntilNextFrame <= 0)
            {
                currentFrame++;
                msUntilNextFrame = msPerFrame;
            }

            if (currentFrame >= numFrames)
            {
                currentFrame = 0;
            }

            currentSprite = sprites[currentFrame];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = Vector2.Zero;
            Rectangle drawRect = currentSprite.GetSourceRect();

            switch (drawOrigin)
            {
                case DrawOrigins.ZERO:
                    origin = Vector2.Zero;
                    break;
                case DrawOrigins.CENTER:
                    origin.X = drawRect.Width / 2;
                    origin.Y = drawRect.Height / 2;
                    break;
                case DrawOrigins.BASE:
                    origin.X = 0;
                    origin.Y = drawRect.Height;
                    break;
                default:
                    origin = Vector2.Zero;
                    break;
            }

            spriteBatch.Draw(currentSprite.GetSheet(),
                             position,
                             currentSprite.GetSourceRect(),
                             currentSprite.mColor,
                             currentSprite.rotation,
                             origin,
                             currentSprite.scale,
                             currentSprite.effect,
                             currentSprite.depth);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            foreach (Sprite sprite in sprites)
            {
                sprite.mPosition = position;
            }
        }

        public void SetColor(Color color)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.mColor = color;
            }
        }

        public void SetEffect(SpriteEffects effect)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.effect = effect;
            }
        }

        public void SetRotation(float rotation)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.rotation = rotation;
            }
        }

        public void SetScale(float scale)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.scale = scale;
            }
        }

        public void SetDepth(float depth)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.depth = depth;
            }
        }

        public void SetDrawOrigin(DrawOrigins drawOrigin)
        {
            this.drawOrigin = drawOrigin;
        }

        public void SetFPS(int FPS)
        {
            SetFPS((float)FPS);
        }

        public void SetFPS(float FPS)
        {
            msPerFrame = (int)(1f / (FPS / 1000f));
        }

        public Sprite GetCurrentSprite()
        {
            return currentSprite;
        }
    }
}
