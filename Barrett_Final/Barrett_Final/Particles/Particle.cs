using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Shaders;

namespace SciFiDemo.Particles
{
    class Particle
    {
        public int mAge;
        Vector2 mPosition;
        Vector2 mVelocity;
        Vector2 mAcceleration;

        float mDampening;
        float mRotation;
        float mRotationVelocity;
        float mRotationDampening;

        float mScale;
        float mScaleVel;
        float mScaleAcc;
        float mScaleMax;

        Color mColor;
        Color mInitColor;
        Color mFinalColor;

        public int mFadeAge;

        LightSource mLight;

        Sprite mSprite;

        public Particle()
        {
            mSprite = new Sprite();
            mLight = new LightSource();
        }

        public void Create(Texture2D texture, int age, Vector2 position,
                            Vector2 velocity, Vector2 acceleration, float dampening,
                            float initRotation, float rotationVelocity, float rotationDampening,
                            float initScale, float initScaleVel, float initScaleAcc, float maxScale,
                            Color initColor, Color finalColor, int fadeAge)
        {
            mSprite.Create(texture);
            mAge = age;
            mPosition = position;
            mVelocity = velocity;
            mAcceleration = acceleration;
            mDampening = dampening;
            mRotation = initRotation;
            mRotationVelocity = rotationVelocity;
            mRotationDampening = rotationDampening;
            mScale = initScale;
            mScaleVel = initScaleVel;
            mScaleAcc = initScaleAcc;
            mScaleMax = maxScale;
            mInitColor = initColor;
            mFinalColor = finalColor;
            mFadeAge = fadeAge;
            mLight.mPos = position;
            mLight.mRadius = 12f * initScale;
        }

        public void UpdatePosition(GameTime gameTime)
        {
            mVelocity *= mDampening;
            mVelocity += (mAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mPosition += (mVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mLight.mPos = mPosition;

            mSprite.mPosition = mPosition;
        }

        public void UpdateRotation(GameTime gameTime)
        {
            mRotation *= mRotationDampening;
            mRotation += (mRotationVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mSprite.rotation = mRotation;
        }

        public void UpdateScale(GameTime gameTime)
        {
            mScaleVel += (mScaleAcc * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mScale += (mScaleVel * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mScale = MathHelper.Clamp(mScale, 0.0f, mScaleMax);
            mSprite.scale = mScale;
        }

        public void UpdateColor(GameTime gameTime)
        {
            if ((mAge > mFadeAge) && (mFadeAge != 0))
            {
                mColor = mInitColor;
            }
            else
            {
                float amtInit = (float)mAge / (float)mFadeAge;
                float amtFinal = 1.0f - amtInit;

                mColor.R = (byte)((amtInit * mInitColor.R) + (amtFinal * mFinalColor.R));
                mColor.G = (byte)((amtInit * mInitColor.G) + (amtFinal * mFinalColor.G));
                mColor.B = (byte)((amtInit * mInitColor.B) + (amtFinal * mFinalColor.B));
                mColor.A = (byte)((amtInit * mInitColor.A) + (amtFinal * mFinalColor.A));
            }

            mSprite.mColor = mColor;
        }

        public void Update(GameTime gameTime)
        {
            if (mAge < 0)
                return;

            mAge -= gameTime.ElapsedGameTime.Milliseconds;

            UpdatePosition(gameTime);
            UpdateRotation(gameTime);
            UpdateScale(gameTime);
            UpdateColor(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            if (mAge < 0)
                return;

            mSprite.Draw(batch);
        }

        public LightSource GetLightSource()
        {
            return mLight;
        }
    }
}
