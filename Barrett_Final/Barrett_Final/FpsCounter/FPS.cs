using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DynamicZoom.FpsCounter
{
    class FPS
    {
        double elapsedMS = 0;
        int frameCount = 0;
        int fps = 0;
        SpriteFont font;
        Color color;
        Vector2 pos;

        public FPS()
        {
            elapsedMS = 0;
            frameCount = 0;
            fps = 0;
            color = Color.White;
            pos = new Vector2(10, 10);
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("FPSfont");
        }

        public void Update(GameTime gameTime)
        {
            elapsedMS += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMS > 1000)
            {
                elapsedMS -= 1000;
                fps = frameCount;
                frameCount = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frameCount++;

            spriteBatch.DrawString(font, "FPS: " + fps, pos, color);
        }
    }
}
