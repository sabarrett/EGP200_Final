using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SciFiDemo.Particles
{
    class CircleEffect : ParticleEffect
    {
        public CircleEffect(Vector2 origin)
            : base(origin)
        {
            // Empty
        }

        public CircleEffect()
            : base()
        {
            // Empty
        }

        public override void Initialize()
        {
            mEffectDuration = 10000;
            mNewParticleAmount = 3;
            mBurstFrequencyMS = 16;
            mBurstCountdownMS = mBurstFrequencyMS;

            mRadius = 50;
            mBlendState = BlendState.Additive;

            base.Initialize();
        }

        public override void CreateParticle()
        {
            particleTexture = circleTexture;

            int initAge = 3000;
            int fadeAge = 2750;

            Vector2 initPos = mOrigin;
            Vector2 offset = new Vector2(((float)(myRandom.Next((int)mRadius) * Math.Cos(myRandom.Next(360)))),
                                          ((float)(myRandom.Next((int)mRadius) * Math.Sin(myRandom.Next(360)))));
            initPos += offset;

            Vector2 initVel = new Vector2((float)(myRandom.Next(100, 100)),
                                          (float)(-300.0f));
            Vector2 initAcc = new Vector2(-150, 150);
            float initDamp = 1.0f;

            float initRot = 0f;
            float initRotVel = 0f;
            float initRotDamp = 1.0f;

            float initScale = 0.5f;
            float initScaleVel = 1.05f;
            float initScaleAcc = 1.0f;
            float maxScale = 5.0f;

            Color initColor = Color.Gray;
            Color finalColor = Color.Gold;
            finalColor.A = 0;

            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            mParticles.Add(tempParticle);

            base.CreateParticle();
        }
    }
}
