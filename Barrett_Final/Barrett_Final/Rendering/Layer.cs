using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Rendering;

namespace SciFiDemo
{
    class Layer
    {
        Camera2d mCamera;
        public Vector2 mParallax;
        List<Drawable> mSprites;

        public Layer(Camera2d camera)
        {
            mCamera = camera;
            mParallax = Vector2.One;
            mSprites = new List<Drawable>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, mCamera.GetTransform(mParallax));

            foreach (Drawable sprite in mSprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void AddSprite(Drawable sprite)
        {
            mSprites.Add(sprite);
        }

        public void ClearAll()
        {
            mSprites.Clear();
        }
    }
}
