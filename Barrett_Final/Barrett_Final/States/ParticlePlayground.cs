using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SciFiDemo.Particles;

namespace SciFiDemo.States
{
    class ParticlePlayground : GameState
    {
        EffectManager effects;

        Sprite player;

        int msSinceNewEffect = 0;

        public ParticlePlayground(EventHandler screenEvent)
            : base(screenEvent)
        {
            player = new Sprite();
            player.mPosition = new Vector2(500, 360 - 128);
            effects = new EffectManager();

            effects.AddEffect(EffectTypes.BURST, new Vector2(640, 360));
            effects.AddEffect(EffectTypes.SIDE_BURST, new Vector2(320, 200));

            mBgColor = Color.CornflowerBlue;
        }

        public void LoadContent(ContentManager content)
        {
            player.Create(content.Load<Texture2D>("run_cycle"), new Rectangle(0, 0, 128, 128));
            effects.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            effects.Update(gameTime);

            msSinceNewEffect += gameTime.ElapsedGameTime.Milliseconds;

            if (msSinceNewEffect > 20000)
            {
                effects.AddEffect(EffectTypes.BURST, new Vector2(640, 360));
                effects.AddEffect(EffectTypes.SIDE_BURST, new Vector2(320, 200));
                msSinceNewEffect = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            spriteBatch.End();

            effects.Draw(spriteBatch, Matrix.Identity);

            base.Draw(spriteBatch);
        }
    }
}
