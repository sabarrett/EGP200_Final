using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SciFiDemo.Particles
{
    class SidewaysBurst : ParticleEffect
    {
        public SidewaysBurst()
            : base()
        {

        }

        public SidewaysBurst(Vector2 origin)
            : base(origin)
        {

        }

        public override void Initialize()
        {
            mEffectDuration = 20000;
            mNewParticleAmount = 15;
            mBurstFrequencyMS = 2000;
            mBurstCountdownMS = 0;

            mRadius = 25;
            mBlendState = BlendState.AlphaBlend;

            base.Initialize();
        }

        public override void CreateParticle()
        {
            particleTexture = circleTexture;

            int initAge = mBurstFrequencyMS;
            int fadeAge = initAge;

            Vector2 initPos = mOrigin;
            Vector2 offset = new Vector2(((float)(myRandom.Next((int)mRadius) * Math.Cos(myRandom.Next(360)))),
                                          ((float)(myRandom.Next((int)mRadius) * Math.Sin(myRandom.Next(360)))));
            initPos += offset;

            Vector2 initVel = new Vector2((float)(myRandom.Next(400, 600)),
                                          (float)(myRandom.Next(-150, 150)));
            Vector2 initAcc = new Vector2(0, 2000);
            float initDamp = 1.0f;

            float initRot = 0f;
            float initRotVel = 0f;
            float initRotDamp = 1.0f;

            float initScale = 0.80f;
            float initScaleVel = 0.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 5.0f;

            Color initColor = Color.White;
            Color finalColor = Color.White;
            finalColor.A = 0;

            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            mParticles.Add(tempParticle);

            base.CreateParticle();
        }
    }
}
