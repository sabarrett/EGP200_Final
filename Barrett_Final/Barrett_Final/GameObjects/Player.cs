using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SciFiDemo.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SciFiDemo.GameObjects;
using SciFiDemo.Rendering;

namespace SciFiDemo
{
    class Player : Drawable
    {
        public delegate void CreateMissileDelegate(Vector2 position, Directions direction);
        public delegate bool DoesMissileExistDelegate();

        public CreateMissileDelegate CreateMissile;
        public DoesMissileExistDelegate DoesMissileExist;

        InputFunctions inputFunctions;

        Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;

        Animation runAnimation;
        Animation idleAnimation;
        Animation currentAnimation;

        Rectangle hitBox;

        double idleTime = 0.0;

        bool facingRight = true;
        bool isJumping = true;

        float moveSpeed = 500f; // pixels per second
        float gravity = 50f; // pixels per second
        float jumpSpeed = 1000f; // pixels per second

        public Player()
        {
            inputFunctions = new InputFunctions();
            AttachInputFunctions();

            position = Vector2.Zero;
            velocity = Vector2.Zero;
            acceleration = new Vector2(0, gravity);
            runAnimation = new Animation();
            idleAnimation = new Animation();
            currentAnimation = runAnimation;
        }

        public void LoadContent(ContentManager content)
        {
            Texture2D runCycleTexture = content.Load<Texture2D>("run_cycle");

            for (int i = 0; i < 12; i++)
            {
                Rectangle sourceRect = new Rectangle(i * 128, 0, 128, 128);
                Sprite nextFrame = new Sprite(runCycleTexture, sourceRect);
                runAnimation.AddSprite(nextFrame);
            }

            idleAnimation.AddSprite(new Sprite(runCycleTexture, new Rectangle(128 * 6, 0, 128, 128)));
        }

        public void Update(GameTime gameTime)
        {
            AdjustMovement(gameTime);

            UpdateCurrentAnimation(gameTime);

            idleTime += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public InputFunctions GetInputFunctions()
        {
            return inputFunctions;
        }

        private void AttachInputFunctions()
        {
            inputFunctions.onMove = onMove;
            inputFunctions.onActionPressed = onActionPressed;
            inputFunctions.onR2Pressed = onR2Pressed;
        }

        public void onMove(Vector2 direction)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                return;

            if (direction.X != 0 || direction.Y != 0)
            {
                idleTime = 0;
            }

            float x = direction.X * moveSpeed;

            velocity.X = x;

            if (facingRight && x < 0)
                facingRight = false;
            else if (!facingRight && x > 0)
                facingRight = true;

            if (x == 0)
                currentAnimation = idleAnimation;
            else
                currentAnimation = runAnimation;
        }

        public void onActionPressed()
        {
            if (!isJumping)
            {
                velocity.Y = -jumpSpeed;
                isJumping = true;
            }

            idleTime = 0;
        }

        public void onR2Pressed()
        {
            Directions direction;

            Vector2 gamePadDirection = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;

            if (Keyboard.GetState().IsKeyDown(Keys.Down) || gamePadDirection.Y < -0.15f)
                direction = Directions.DOWN;
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) || gamePadDirection.Y > 0.15f)
                direction = Directions.UP;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) || gamePadDirection.X < -0.15f)
                direction = Directions.LEFT;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) || gamePadDirection.X > 0.15f)
                direction = Directions.RIGHT;
            else if (facingRight)
                direction = Directions.RIGHT;
            else
                direction = Directions.LEFT;

            CreateMissile(position, direction);

            idleTime = 0;
        }

        private void UpdateCurrentAnimation(GameTime gameTime)
        {
            SpriteEffects newEffect;

            if (facingRight)
                newEffect = SpriteEffects.None;
            else
                newEffect = SpriteEffects.FlipHorizontally;

            currentAnimation.SetEffect(newEffect);

            currentAnimation.SetPosition(position);
            currentAnimation.Update(gameTime);
        }

        private void UpdateAllAnimations(GameTime gameTime)
        {
            runAnimation.SetPosition(position);
            runAnimation.Update(gameTime);

            idleAnimation.SetPosition(position);
            idleAnimation.Update(gameTime);
        }

        private void AdjustMovement(GameTime gameTime)
        {
            velocity += acceleration;

            Vector2 newPosition = position + (velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds));

            position = CheckCollisionAndGetPosition(newPosition);
        }

        private Vector2 CheckCollisionAndGetPosition(Vector2 newPosition)
        {
            Vector2 correctedPosition = newPosition;

            if (newPosition.Y > 595)
            {
                correctedPosition.Y = 595;
                isJumping = false;
            }

            if (newPosition.X > 1890)
            {
                correctedPosition.X = 1890;
            }
            else if (newPosition.X < 30)
            {
                correctedPosition.X = 30;
            }

            return correctedPosition;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            idleAnimation.SetPosition(position);
            runAnimation.SetPosition(position);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetWidth()
        {
            return runAnimation.GetCurrentSprite().GetSourceRect().Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch);
        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public double GetIdleTime()
        {
            return idleTime;
        }
    }

    
}
