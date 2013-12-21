using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SciFiDemo.Shaders;

namespace SciFiDemo.Particles
{
    class ParticleEffect
    {
        public Texture2D particleTexture;
        protected static Texture2D circleTexture;

        public Vector2 mOrigin;
        protected float mRadius;
        
        protected int mEffectDuration;
        protected int mNewParticleAmount;
        protected int mBurstFrequencyMS;
        protected int mBurstCountdownMS;
        
        protected Random myRandom;

        public List<Particle> mParticles;

        public BlendState mBlendState;

        public ParticleEffect(Vector2 origin)
        {
            mOrigin = origin;
            mParticles = new List<Particle>();
            myRandom = new Random();
            particleTexture = circleTexture;
        }

        public ParticleEffect() :
            this(Vector2.Zero)
        {
            // Empty
        }

        public static void LoadContent(ContentManager content)
        {
            circleTexture = content.Load<Texture2D>("whiteCircle-16x16");
        }

        public virtual void Initialize()
        {

        }

        public virtual void CreateParticle()
        {

        }

        public void Update(GameTime gameTime)
        {
            mEffectDuration -= gameTime.ElapsedGameTime.Milliseconds;
            mBurstCountdownMS -= gameTime.ElapsedGameTime.Milliseconds;

            if ((mBurstCountdownMS <= 0) && (mEffectDuration >= 0))
            {
                for (int i = 0; i < mNewParticleAmount; i++)
                {
                    CreateParticle();
                }

                mBurstCountdownMS = mBurstFrequencyMS;
            }

            for (int i = mParticles.Count() - 1; i >= 0; i--)
            {
                mParticles[i].Update(gameTime);

                if (mParticles[i].mAge <= 0)
                    mParticles.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, mBlendState, null, null, null, null, transform);

            foreach (Particle p in mParticles)
            {
                p.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public bool isAlive()
        {
            if (mEffectDuration > 0)
                return true;
            if (mParticles.Count() > 0)
                return true;
            return false;
        }

        public List<LightSource> GetLightSources()
        {
            List<LightSource> lights = new List<LightSource>();

            foreach (Particle p in mParticles)
                lights.Add(p.GetLightSource());

            return lights;
        }
    }
}
