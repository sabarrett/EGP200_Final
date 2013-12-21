using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Input;
using Scripts;

namespace SciFiDemo.States
{
    enum MainMenuOptions
    {
        START = -1,
        PLAY,
        PARTICLE_EDITOR,
        CHANGE_LANGUAGE,
        QUIT,
        NUM_OPTIONS
    }

    class MainMenu : GameState
    {
        InputFunctions inputFunctions;
        MainMenuOptions currentSelection;
        SpriteFont font;

        Vector2 mScreenCenter;
        Vector2 mTextPosition;
        Vector2 widestItemDimensions = Vector2.Zero;

        Sprite mCursor;

        float totalHeight = 0f;

        const float MARGIN = 10f; // in pixels. Universal number for adding negative space.

        Color fontColor = Color.White;

        ScriptHolder[] scripts;

        bool analogStickIsNeutral = true;

        string[] options;

        public MainMenu(EventHandler screenEvent)
            : base(screenEvent)
        {
            inputFunctions = new InputFunctions();

            mCursor = new Sprite();

            inputFunctions.onActionPressed = onActionPressed;
            inputFunctions.onMove = onMove;
            mBgColor = Color.Black;

            currentSelection = MainMenuOptions.PLAY;
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("guiFont");

            Rectangle srcRect = new Rectangle(0, 0, 64, 21);
            mCursor.Create(content.Load<Texture2D>("rocket-64x21"), srcRect);

            scripts = content.Load<ScriptHolder[]>("languages");

            scripts[language].mainMenuScript.Refresh();
            options = scripts[language].mainMenuScript.GetItems();

            DetermineDerivedAttributes();
        }

        public void SetCenter(Vector2 center)
        {
            mScreenCenter = center;
        }

        public MainMenuOptions GetCurrentSelection()
        {
            return currentSelection;
        }

        // Override Methods ////////////////////////////////////////////////////////////

        public override void AttachInput(InputListener input)
        {
            input.AttachFunctions(inputFunctions);

            base.AttachInput(input);
        }

        public override void DetachInput(InputListener input)
        {
            input.DetachFunctions(inputFunctions);

            base.DetachInput(input);
        }

        public override void Update(GameTime gameTime)
        {
            DetermineCursorPosition();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < scripts[language].mainMenuScript.GetItems().Length; i++)
            {
                Vector2 pos = Vector2.Zero;

                pos.X = mTextPosition.X;
                pos.Y = mTextPosition.Y + (widestItemDimensions.Y * i + MARGIN);

                spriteBatch.DrawString(font, options[i], pos, fontColor);
            }

            mCursor.DrawAtCenter(spriteBatch);

#if WINDOWS
            spriteBatch.DrawString(font, "Z = " + scripts[language].mainMenuScript.Jump, new Vector2(mScreenCenter.X - 300, mScreenCenter.Y + 200), Color.White);
            spriteBatch.DrawString(font, "X = " + scripts[language].mainMenuScript.Shoot, new Vector2(mScreenCenter.X, mScreenCenter.Y + 200), Color.White);
#elif XBOX
            spriteBatch.DrawString(font, "A = " + scripts[language].mainMenuScript.Jump, new Vector2(mScreenCenter.X - 300, mScreenCenter.Y + 200), Color.White);
            spriteBatch.DrawString(font, "RB = " + scripts[language].mainMenuScript.Shoot, new Vector2(mScreenCenter.X, mScreenCenter.Y + 200), Color.White);
#endif

            base.Draw(spriteBatch);
        }

        // Private Methods ///////////////////////////////////////////////////////////////

        private void DetermineDerivedAttributes()
        {
            for (int i = 0; i < scripts[language].mainMenuScript.GetItems().Length; i++)
            {
                Vector2 currStringDimensions = font.MeasureString(options[i]);

                if (widestItemDimensions.X < currStringDimensions.X)
                {
                    widestItemDimensions = currStringDimensions;
                }

                totalHeight += currStringDimensions.Y;
            }

            mTextPosition.X = mScreenCenter.X - (widestItemDimensions.X / 2);
            mTextPosition.Y = mScreenCenter.Y - (totalHeight / 2);

            DetermineCursorPosition();
        }

        private void DetermineCursorPosition()
        {
            mCursor.mPosition.X = mTextPosition.X - (MARGIN + (mCursor.GetSourceRect().Width / 2));
            mCursor.mPosition.Y = mTextPosition.Y + (widestItemDimensions.Y * (float)currentSelection) + MARGIN + (widestItemDimensions.Y / 2);
        }

        private void ChangeCurrentSelection(int direction)
        {
            int modifiedDirection = (int)MathHelper.Clamp(direction, -1, 1);

            currentSelection += direction;

            currentSelection = (MainMenuOptions)MathHelper.Clamp((float)currentSelection, (float)MainMenuOptions.START + 1, (float)MainMenuOptions.NUM_OPTIONS - 1);
        }

        // Input Methods /////////////////////////////////////////////////////////////////

        public void onActionPressed()
        {
            if (currentSelection == MainMenuOptions.CHANGE_LANGUAGE)
            {
                if (language == 0)
                    language = 1;
                else
                    language = 0;

                scripts[language].mainMenuScript.Refresh();
                options = scripts[language].mainMenuScript.GetItems();
            }
            else
            {
                mScreenEvent.Invoke(this, EventArgs.Empty);
            }
        }

        public void onMove(Vector2 direction)
        {
            if (analogStickIsNeutral)
            {
                if (direction.Y > 0.15f)
                {
                    ChangeCurrentSelection(1);
                    analogStickIsNeutral = false;
                }
                else if (direction.Y < -0.15f)
                {
                    ChangeCurrentSelection(-1);
                    analogStickIsNeutral = false;
                }
            }

            if (direction.Y == 0)
                analogStickIsNeutral = true;
        }
    }
}
