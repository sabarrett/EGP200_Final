using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SciFiDemo.Input;

namespace SciFiDemo.States
{
    class ScreenManager
    {
        // Screens
        GameState currentScreen;
        TitleScreen titleScreen;
        GameplayScreen gameplayScreen;
        PauseScreen pauseScreen;
        MainMenu mainMenu;
        ParticleEditor particleEditor;

        // Input Listener
        InputListener input;

        // Game Time Tracker
        GameTime currentGameTime;

        // Rendering Variables
        GraphicsDevice mGraphicsDevice;
        
        // Screen Dimensions
        Vector2 mScreenCenter;

        List<Shaders.LightSource> lights;

        // To quit or not to quit?
        bool exit = false;

        int mScreenWidth = 0; // in pixels
        int mScreenHeight = 0; // in pixels



        public ScreenManager(GraphicsDevice graphicsDevice)
        {
            mGraphicsDevice = graphicsDevice;
            mScreenWidth = graphicsDevice.Viewport.Width;
            mScreenHeight = graphicsDevice.Viewport.Height;
            mScreenCenter = new Vector2(mScreenWidth / 2, mScreenHeight / 2);

            lights = new List<Shaders.LightSource>();

            input = new InputListener();

            titleScreen = new TitleScreen(TitleScreenEvent);
            gameplayScreen = new GameplayScreen(GameplayScreenEvent, mGraphicsDevice);
            pauseScreen = new PauseScreen(PauseScreenEvent);
            mainMenu = new MainMenu(MainMenuEvent);
            particleEditor = new ParticleEditor(ParticleEvent);

            titleScreen.SetCenter(mScreenCenter);
            pauseScreen.SetCenter(mScreenCenter);
            mainMenu.SetCenter(mScreenCenter);
            particleEditor.SetCenter(mScreenCenter);

            currentScreen = titleScreen;
            currentScreen.AttachInput(input);

            SetCustomKeys();
        }

        public void LoadContent(ContentManager content)
        {
            titleScreen.SetCenter(new Vector2(mScreenWidth / 2, mScreenHeight / 2));
            titleScreen.LoadContent(content);
            gameplayScreen.LoadContent(content);
            pauseScreen.LoadContent(content);
            mainMenu.LoadContent(content);
            particleEditor.LoadContent(content);
        }

        public bool Update(GameTime gameTime)
        {
            input.Update();

            currentScreen.Update(gameTime);

            currentGameTime = gameTime;

            return exit;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentScreen != gameplayScreen)
            {
                mGraphicsDevice.Clear(currentScreen.GetBgColor());
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            }

            currentScreen.Draw(spriteBatch);

            if (currentScreen != gameplayScreen)
                spriteBatch.End();
        }

        private void SwitchInputControl(GameState toState)
        {
            currentScreen.DetachInput(input);
            toState.AttachInput(input);
        }

        private void SwitchState(GameState toState)
        {
            // Give input control to the new state
            SwitchInputControl(toState);

            // Switch the state
            currentScreen = toState;

            currentScreen.Update(currentGameTime);
        }

        // Screen Events //////////////////////////////////////////////////////

        public void TitleScreenEvent(object obj, EventArgs e)
        {
            SwitchState(mainMenu);
        }

        public void GameplayScreenEvent(object obj, EventArgs e)
        {
            SwitchState(pauseScreen);
        }

        public void PauseScreenEvent(object obj, EventArgs e)
        {
            PauseMenuOptions option = pauseScreen.GetCurrentSelection();

            switch (option)
            {
                case PauseMenuOptions.RESUME:
                    {
                        SwitchState(gameplayScreen);
                    }
                    break;
                case PauseMenuOptions.QUIT:
                    {
                        SwitchState(mainMenu);
                    }
                    break;
            }
        }

        public void MainMenuEvent(object obj, EventArgs e)
        {
            MainMenuOptions option = mainMenu.GetCurrentSelection();

            switch (option)
            {
                case MainMenuOptions.PLAY:
                    {
                        SwitchState(gameplayScreen);
                    }
                    break;
                case MainMenuOptions.PARTICLE_EDITOR:
                    {
                        SwitchState(particleEditor);
                    }
                    break;
                case MainMenuOptions.QUIT:
                    {
                        exit = true;
                    }
                    break;
            }
        }

        public void ParticleEvent(object obj, EventArgs e)
        {
            SwitchState(mainMenu);
        }

        // Private Functions ///////////////////////////////////////////////////////////////////

        private void SetCustomKeys()
        {
            input.actionKey = Keys.Z;
            input.r2Key = Keys.X;
            input.backKey = Keys.X;
        }
    }
}
