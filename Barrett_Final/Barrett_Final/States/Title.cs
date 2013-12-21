using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SciFiDemo.States
{
    class TitleScreen : GameState
    {
        double elapsedSeconds;

        SpriteFont font;
        Vector2 position;
        Vector2 screenCenter;

        const double MAX_DISPLAY_TIME = 3; // in seconds
        string DISPLAY_TEXT = "SciFi Graphics Demo";
        Color FONT_COLOR = Color.White;

        public TitleScreen(EventHandler screenEvent)
            : base(screenEvent)
        {
            position = new Vector2(50, 50);
            screenCenter = Vector2.Zero;
            mBgColor = Color.Black;
        }

        public void SetCenter(Vector2 center)
        {
            screenCenter = center;
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("guiFont");

            Vector2 dimensions = font.MeasureString(DISPLAY_TEXT);

            position = screenCenter - (dimensions / 2);
        }

        public override void Update(GameTime gameTime)
        {
            elapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedSeconds > MAX_DISPLAY_TIME)
            {
                mScreenEvent.Invoke(this, new EventArgs());
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, DISPLAY_TEXT, position, FONT_COLOR);

            base.Draw(spriteBatch);
        }
    }
}
