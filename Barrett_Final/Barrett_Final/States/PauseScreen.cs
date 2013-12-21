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
    enum PauseMenuOptions
    {
        START = -1,
        RESUME,
        QUIT,
        NUM_OPTIONS
    }

    class PauseScreen : GameState
    {
        InputFunctions inputFunctions;
        PauseMenuOptions currentSelection;
        SpriteFont font;

        Vector2 mScreenCenter;
        Vector2 mTextPosition;
        Vector2 widestItemDimensions = Vector2.Zero;

        Sprite mCursor;

        float totalHeight = 0f;

        bool analogStickIsNeutral = true;

        string[] options = { "Resume", "Exit" };

        const float MARGIN = 10f; // in pixels. Universal number for adding negative space.

        ScriptHolder[] scripts;

        Color fontColor = Color.Black;

        public PauseScreen(EventHandler screenEvent)
            : base(screenEvent)
        {
            mBgColor = Color.AliceBlue;

            mCursor = new Sprite();

            inputFunctions = new InputFunctions();
            inputFunctions.onActionPressed = onActionPressed;
            inputFunctions.onMove = onMove;

            currentSelection = PauseMenuOptions.RESUME;
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("guiFont");

            Rectangle srcRect = new Rectangle(0, 0, 64, 21);
            mCursor.Create(content.Load<Texture2D>("rocket-64x21"), srcRect);

            scripts = content.Load<ScriptHolder[]>("languages");

            scripts[language].pauseMenuScript.Refresh();
            options = scripts[language].pauseMenuScript.GetItems();

            DetermineDerivedAttributes();
        }

        public void SetCenter(Vector2 center)
        {
            mScreenCenter = center;
        }

        public PauseMenuOptions GetCurrentSelection()
        {
            return currentSelection;
        }

        // Override Methods ///////////////////////////////////////////////////////////////////////////////////

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

            scripts[language].pauseMenuScript.Refresh();
            options = scripts[language].pauseMenuScript.GetItems();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < scripts[language].pauseMenuScript.GetItems().Length; i++)
            {
                Vector2 pos = Vector2.Zero;

                pos.X = mTextPosition.X;
                pos.Y = mTextPosition.Y + (widestItemDimensions.Y * i) + MARGIN;

                spriteBatch.DrawString(font, options[i], pos, fontColor);
            }

            mCursor.DrawAtCenter(spriteBatch);

            base.Draw(spriteBatch);
        }

        // Private Methods /////////////////////////////////////////////////////////////////////////////////////////

        private void DetermineDerivedAttributes()
        {
            for (int i = 0; i < scripts[language].pauseMenuScript.GetItems().Length ; i++)
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

            currentSelection = (PauseMenuOptions)MathHelper.Clamp((float)currentSelection, (float)PauseMenuOptions.START + 1, (float)PauseMenuOptions.NUM_OPTIONS - 1);
        }

        // Input Functions ///////////////////////////////////////////////////

        public void onActionPressed()
        {
            mScreenEvent.Invoke(this, EventArgs.Empty);
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
