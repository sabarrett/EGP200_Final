using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SciFiDemo
{
    class VerticalBlock : Block
    {
        public VerticalBlock()
            : base()
        {
            // Reverse the width/height of the parent's hitbox
            hitbox = new Rectangle(0, 0, 64, 128);
        }

        public override void LoadContent(ContentManager content)
        {
            Texture2D texture = content.Load<Texture2D>("vertblock");
            Rectangle sourceRect = new Rectangle(0, 0, 64, 128);

            sprite = new Sprite(texture, sourceRect);
            SetPosition(position);
        }
    }
}
