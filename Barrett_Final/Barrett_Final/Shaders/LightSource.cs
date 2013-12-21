using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SciFiDemo.Shaders
{
    class LightSource
    {
        public Vector2 mPos;
        public float mRadius;

        public LightSource(Vector2 pos, float radius)
        {
            mPos = pos;
            mRadius = radius;
        }

        public LightSource(Vector2 pos)
            : this(pos, 0f)
        {

        }

        public LightSource(float radius)
            : this(Vector2.Zero, radius)
        {

        }

        public LightSource()
            : this(Vector2.Zero, 0f)
        {

        }
    }
}
