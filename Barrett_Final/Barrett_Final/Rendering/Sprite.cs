using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Rendering;

namespace SciFiDemo
{
    class Sprite : Drawable
    {
        Texture2D mSheet;
        Rectangle mSourceRect;

        public Vector2 mPosition;
        public Color mColor;
        public SpriteEffects effect;

        public float rotation = 0f;
        public float scale = 1f;
        public float depth = 1f;

        public Sprite()
        {
            mPosition = Vector2.Zero;
            mColor = Color.White;
            effect = SpriteEffects.None;
            mSheet = null;
            mSourceRect = Rectangle.Empty;
        }

        public Sprite(Texture2D sheet)
        {
            mPosition = Vector2.Zero;
            mColor = Color.White;
            effect = SpriteEffects.None;
            mSheet = sheet;
            mSourceRect = Rectangle.Empty;
        }

        public Sprite(Texture2D sheet, Rectangle sourceRect)
        {
            mPosition = Vector2.Zero;
            mColor = Color.White;
            effect = SpriteEffects.None;
            mSheet = sheet;
            mSourceRect = sourceRect;
        }

        public void Create(Texture2D texture)
        {
            mSheet = texture;
            mSourceRect = Rectangle.Empty;
        }

        public void Create(Texture2D texture, Rectangle sourceRect)
        {
            mSheet = texture;
            mSourceRect = sourceRect;
        }

        public Texture2D GetSheet()
        {
            return mSheet;
        }

        public Rectangle GetSourceRect()
        {
            return mSourceRect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (mSourceRect == Rectangle.Empty)
            {
                spriteBatch.Draw(mSheet,
                             mPosition,
                             null,
                             mColor,
                             rotation,
                             Vector2.Zero,
                             scale,
                             effect,
                             depth);
            }

            else
            {
                spriteBatch.Draw(mSheet,
                                 mPosition,
                                 mSourceRect,
                                 mColor,
                                 rotation,
                                 Vector2.Zero,
                                 scale,
                                 effect,
                                 depth);
            }
        }

        public void DrawAtCenter(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(mSourceRect.Width / 2, mSourceRect.Height / 2);

            if (mSourceRect == Rectangle.Empty)
            {
                spriteBatch.Draw(mSheet,
                             mPosition,
                             null,
                             mColor,
                             rotation,
                             origin,
                             scale,
                             effect,
                             depth);
            }

            else
            {
                spriteBatch.Draw(mSheet,
                                 mPosition,
                                 mSourceRect,
                                 mColor,
                                 rotation,
                                 origin,
                                 scale,
                                 effect,
                                 depth);
            }
        }
    }
}
