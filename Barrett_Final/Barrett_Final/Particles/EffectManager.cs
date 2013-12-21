using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Shaders;

namespace SciFiDemo.Particles
{
    enum EffectTypes
    {
        INVALID = -1,

        CIRCLE,
        BURST,
        SIDE_BURST,
        CUSTOM,

        NUM_EFFECT_TYPES
    }

    class EffectManager
    {
        public List<ParticleEffect> mAllEffects;

        public EffectManager()
        {
            mAllEffects = new List<ParticleEffect>();
        }

        public void LoadContent(ContentManager content)
        {
            ParticleEffect.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            //ParticleCounter.sNumParticles = 0;

            for (int i = mAllEffects.Count() - 1; i >= 0; i--)
            {
                mAllEffects[i].Update(gameTime);

                //ParticleCounter.sNumParticles += mAllEffects[i].mParticles.Count();

                if (!mAllEffects[i].isAlive())
                    mAllEffects.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            foreach (ParticleEffect e in mAllEffects)
            {
                e.Draw(spriteBatch, transform);
            }
        }

        public void AddEffect(EffectTypes type, Vector2 position)
        {
            ParticleEffect tempEffect;

            // Initialize the effect
            switch (type)
            {
                case (EffectTypes.CIRCLE):
                    {
                        tempEffect = new CircleEffect(position);
                    }
                    break;
                case EffectTypes.BURST:
                    {
                       tempEffect = new BurstEffect(position);
                    }
                    break;
                case EffectTypes.SIDE_BURST:
                    {
                        tempEffect = new SidewaysBurst(position);
                    }
                    break;
                case EffectTypes.CUSTOM:
                    {
                        tempEffect = new CustomParticle(position);
                    }
                    break;
                default:
                    {
                        tempEffect = null;
                    }
                    break;
            }

            tempEffect.Initialize();
            mAllEffects.Add(tempEffect);
        }

        public List<LightSource> GetLightSources()
        {
            List<LightSource> lights = new List<LightSource>();

            foreach (ParticleEffect p in mAllEffects)
            {
                List<LightSource> theLights = p.GetLightSources();

                foreach (LightSource light in theLights)
                    lights.Add(light);
            }

            return lights;
        }
    }
}
