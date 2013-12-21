using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Input;
using SciFiDemo.Rendering;

namespace SciFiDemo
{
    class GameState : Drawable
    {
        protected EventHandler mScreenEvent;
        protected Color mBgColor = Color.CornflowerBlue;

        protected static int language = 0;

        public GameState(EventHandler screenEvent)
        {
            mScreenEvent = screenEvent;
        }

        public Color GetBgColor()
        {
            return mBgColor;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void AttachInput(InputListener input)
        {

        }

        public virtual void DetachInput(InputListener input)
        {

        }
    }
}
