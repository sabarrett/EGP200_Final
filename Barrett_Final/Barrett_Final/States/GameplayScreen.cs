using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SciFiDemo.Input;
using SciFiDemo.GameObjects;
using SciFiDemo.Shaders;
using SciFiDemo.Particles;

namespace SciFiDemo.States
{
    class GameplayScreen : GameState
    {
        const float CAMERA_MARGIN = 200;

        bool loadedCustomParticle = false;

        Camera2d camera;
        GraphicsDevice mGraphicsDevice;

        SpriteFont font;

        Texture2D lightSprite;

        Rectangle worldBounds;

        Sprite background;

        Player player;
        List<Block> blocks;

        List<Layer> layers;

        EffectManager particleEffects;

        RenderTarget2D secondBackBuffer;
        RenderTarget2D lightMap;
        RenderTargetBinding[] realBackBuffer;

        Effect lighting;
        EffectParameter lightMapParam;

        List<LightSource> lights;
        LightSource playerLight;

        List<Missile> mMissiles;
        bool doesMissileExist = false;

        public GameplayScreen(EventHandler screenEvent, GraphicsDevice graphicsDevice)
            : base(screenEvent)
        {
            player = new Player();
            background = new Sprite();
            blocks = new List<Block>();
            layers = new List<Layer>();
            lights = new List<LightSource>();
            playerLight = new LightSource();
            particleEffects = new Particles.EffectManager();
            camera = new Camera2d(graphicsDevice.Viewport);

            worldBounds = new Rectangle(0, 0, 1920, 720);

            mMissiles = new List<Missile>();

            mGraphicsDevice = graphicsDevice;
            realBackBuffer = graphicsDevice.GetRenderTargets();
            secondBackBuffer = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            lightMap = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            // Set intitial player position
            player.SetPosition(new Vector2(300, 300));
            player.CreateMissile = CreateMissile;

            lights.Add(new LightSource(new Vector2(500,  400), 150f));
            lights.Add(new LightSource(new Vector2(800,  400), 150f));
            lights.Add(new LightSource(new Vector2(1100, 400), 150f));
            lights.Add(new LightSource(new Vector2(1400, 400), 150f));
            lights.Add(new LightSource(new Vector2(1700, 400), 150f));

            mBgColor = Color.Black;
        }

        public void LoadContent(ContentManager content)
        {
            particleEffects.LoadContent(content);
            particleEffects.AddEffect(Particles.EffectTypes.BURST, new Vector2(150,  650));
            particleEffects.AddEffect(Particles.EffectTypes.BURST, new Vector2(1000, 650));
            particleEffects.AddEffect(Particles.EffectTypes.SIDE_BURST, new Vector2(128, 128));

            lightSprite = content.Load<Texture2D>("whiteCircle");

            lighting = content.Load<Effect>("Shaders/lighting");
            lightMapParam = lighting.Parameters["lightTexture"];

            font = content.Load<SpriteFont>("guiFont");

            player.LoadContent(content);
            Missile.LoadContent(content);
            background.Create(content.Load<Texture2D>("Images/spaceBG-2080x1560"));
            background.mPosition.Y = -720;

            for (int i = 0; i < 6; i++)
            {
                Block vertBlock = new VerticalBlock();
                vertBlock.LoadContent(content);
                vertBlock.SetPosition(new Vector2(0, i * 128));
                blocks.Add(vertBlock);
            }

            for (int i = 0; i < 6; i++)
            {
                Block vertBlock = new VerticalBlock();
                vertBlock.LoadContent(content);
                vertBlock.SetPosition(new Vector2(15 * 128 + 64, i * 128));
                blocks.Add(vertBlock);
            }

            for (int i = 0; i < 15; i++)
            {
                Block block = new Block();
                block.LoadContent(content);
                block.SetPosition(new Vector2(i * 128 + 64, 0));
                blocks.Add(block);
            }

            for (int i = 0; i < 15; i++)
            {
                Block block = new Block();
                block.LoadContent(content);
                block.SetPosition(new Vector2(i * 128 + 64, 704));
                blocks.Add(block);
            }

            Layer layer = new Layer(camera);
            layer.AddSprite(player);

            foreach (Block block in blocks)
                layer.AddSprite(block);

            Layer layer2 = new Layer(camera);
            layer2.AddSprite(background);
            layer2.mParallax = new Vector2(0.5f);

            layers.Add(layer2);
            layers.Add(layer);
        }

        public override void AttachInput(InputListener input)
        {
            input.AttachFunctions(player.GetInputFunctions());
            input.onPausePressed += onPausePressed;
            input.onMove += onMove;

            base.AttachInput(input);
        }

        public override void DetachInput(InputListener input)
        {
            input.DetachFunctions(player.GetInputFunctions());
            input.onPausePressed -= onPausePressed;
            input.onMove -= onMove;

            base.DetachInput(input);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            particleEffects.Update(gameTime);
            UpdateCamera(gameTime);

            if (!loadedCustomParticle && CustomParticle.Exists())
            {
                particleEffects.AddEffect(EffectTypes.CUSTOM, new Vector2(1300, 650));
                loadedCustomParticle = true;
            }

            player.SetPosition(new Vector2(MathHelper.Clamp(player.GetPosition().X, worldBounds.X, worldBounds.X + worldBounds.Width), player.GetPosition().Y));
            camera.mPos = new Vector2(MathHelper.Clamp(camera.mPos.X, worldBounds.X, worldBounds.X + worldBounds.Width), camera.mPos.Y);

            playerLight.mPos = player.GetPosition() + new Vector2(64);
            playerLight.mRadius = 200f;

            foreach (Missile m in mMissiles)
                m.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            List<LightSource> particleLights = new List<LightSource>();

            particleLights = particleEffects.GetLightSources();

            mGraphicsDevice.SetRenderTarget(secondBackBuffer);
            mGraphicsDevice.Clear(mBgColor);

            DrawScene(spriteBatch);

            #region cpuLightMap
            // Draw the light map using the cpu
            mGraphicsDevice.SetRenderTarget(lightMap);
            mGraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, camera.GetTransform(Vector2.One));
            foreach (LightSource light in lights)
            {
                DrawLightSource(spriteBatch, light);
            }
            foreach (LightSource light in particleLights)
            {
                DrawLightSource(spriteBatch, light);
            }

            DrawLightSource(spriteBatch, playerLight);

            spriteBatch.End();

            // End drawing the light map using the cpu
            #endregion

            // Prepare to apply lighting shader
            mGraphicsDevice.SetRenderTargets(realBackBuffer);
            lightMapParam.SetValue(lightMap);

            // Draw scene with lighting shader
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            lighting.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(secondBackBuffer, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        private void DrawScene(SpriteBatch spriteBatch)
        {
            foreach (Layer layer in layers)
                layer.Draw(spriteBatch);

            particleEffects.Draw(spriteBatch, camera.GetTransform(Vector2.One));

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransform(Vector2.One));
            foreach (Missile m in mMissiles)
            {
                m.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        private void DrawLightSource(SpriteBatch spriteBatch, LightSource light)
        {
            spriteBatch.Draw(lightSprite,
                                 new Vector2(light.mPos.X, light.mPos.Y),
                                 null,
                                 Color.White,
                                 0f,
                                 new Vector2(lightSprite.Width / 2, lightSprite.Height / 2),
                                 2 * light.mRadius / lightSprite.Width,
                                 SpriteEffects.None,
                                 1f);
        }

        private void UpdateCamera(GameTime gameTime)
        {
            float cameraX = camera.mPos.X + mGraphicsDevice.Viewport.Width * 0.5f;
            float playerX = player.GetPosition().X + player.GetWidth() * 0.5f;
            float cameraSpeed = (float)(player.GetMoveSpeed() * gameTime.ElapsedGameTime.TotalSeconds);

            if (playerX < cameraX - (mGraphicsDevice.Viewport.Width * 0.5f) + CAMERA_MARGIN)
                camera.Move(new Vector2(-cameraSpeed, 0));
            else if (playerX > cameraX + mGraphicsDevice.Viewport.Width * 0.5f - CAMERA_MARGIN)
                camera.Move(new Vector2(cameraSpeed, 0));
        }

        // Input Listener Functions ////////////////////////////////////

        public void onPausePressed()
        {
            mScreenEvent.Invoke(this, EventArgs.Empty);
        }

        // Delegate Functions ///////////////////////////////////////////

        public void CreateMissile(Vector2 position, Directions direction)
        {
            Missile missile = new Missile();
            missile.SetPosition(player.GetPosition());
            missile.SetVelocity(direction);

            mMissiles.Add(missile);
        }

        public bool DoesMissileExist()
        {
            return doesMissileExist;
        }

        // Debug Input Listeners

        public void onMove(Vector2 direction)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            {
                camera.Zoom += direction.Y * 0.1f;
                camera.Rotation += direction.X * 0.1f;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                camera.Move(direction * 3f);
            }
        }
    }
}
