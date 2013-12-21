using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SciFiDemo.Particles
{
    class CustomParticle : ParticleEffect
    {
        static float sScale;
        static Color sStartColor;
        static Color sEndColor;
        static bool sInitialized;

        public CustomParticle()
            : base()
        {

        }

        public CustomParticle(Vector2 origin)
            : base(origin)
        {
            sInitialized = false;
        }

        public static void SetAttributes(float scale, Color startColor, Color endColor)
        {
            sScale = scale;
            sStartColor = startColor;
            sEndColor = endColor;
            sInitialized = true;
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

            Vector2 initVel = new Vector2((float)(myRandom.Next(-100, 100)),
                                          (float)(myRandom.Next(-600, -500)));
            Vector2 initAcc = new Vector2(0, 2000);
            float initDamp = 1.0f;

            float initRot = 0f;
            float initRotVel = 0f;
            float initRotDamp = 1.0f;

            float initScale = sScale;
            float initScaleVel = 0.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 5.0f;

            Color initColor = sStartColor;
            Color finalColor = sEndColor;
            finalColor.A = 0;

            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            mParticles.Add(tempParticle);

            base.CreateParticle();
        }

        public static bool Exists()
        {
            return sInitialized;
        }
    }
}
