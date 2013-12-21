using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SciFiDemo
{
    class Camera2d
    {
        protected float mZoom;
        public Matrix mTransform;
        public Vector2 mPos;
        Vector2 mOrigin;
        protected float mRotation;

        public Camera2d(Viewport viewport)
        {
            mZoom = 1f;
            mRotation = 0f;
            mOrigin = new Vector2(viewport.Width / 2, viewport.Height / 2);
            mPos = Vector2.Zero;
        }

        public float Zoom
        {
            get { return mZoom; }
            set { mZoom = value; if (mZoom < 0.1f) mZoom = 0.1f; }
        }

        public float Rotation
        {
            get { return mRotation; }
            set { mRotation = value; }
        }

        public void Move(Vector2 translation)
        {
            translation = Vector2.Transform(translation, Matrix.CreateRotationZ(-Rotation));
            mPos += translation;
        }

        public Vector2 Pos
        {
            get { return mPos; }
            set { mPos = value; }
        }

        public Matrix GetTransform(Vector2 parallax)
        {
            mTransform = Matrix.CreateTranslation(new Vector3(-mPos * parallax, 0.0f)) *
                         Matrix.CreateTranslation(new Vector3(-mOrigin, 0.0f)) *
                         Matrix.CreateRotationZ(mRotation) *
                         Matrix.CreateScale(new Vector3(mZoom, mZoom, 0)) *
                         Matrix.CreateTranslation(new Vector3(mOrigin, 0));

            return mTransform;
        }
    }
}
