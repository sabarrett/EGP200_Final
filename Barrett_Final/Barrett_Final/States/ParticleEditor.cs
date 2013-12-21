using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SciFiDemo.Input;
using SciFiDemo.Particles;
using Scripts;

namespace SciFiDemo.States
{
    enum ParticleEditorOptions
    {
        START = -1,
        START_COLOR,
        END_COLOR,
        SIZE,
        QUIT,
        NUM_OPTIONS
    }

    class ParticleEditor : GameState
    {
        InputFunctions inputFunctions;
        ParticleEditorOptions currentSelection;
        SpriteFont font;

        Vector2 mScreenCenter;
        Vector2 mTextPosition;
        Vector2 widestItemDimensions = Vector2.Zero;

        Sprite mCursor;

        // Particle info
        Color startColor;
        Color endColor;
        float size;

        string[] changiningInformation;

        EffectManager effect;

        float totalHeight = 0f;

        const float MARGIN = 10f; // in pixels. Universal number for adding negative space.

        Color fontColor = Color.White;

        ScriptHolder[] scripts;

        bool analogStickIsNeutral = true;

        string[] options;
        string[] colorStrings;
        Color[] colors;
        int currentStartColor;
        int currentEndColor;

        public ParticleEditor(EventHandler screenEvent)
            : base(screenEvent)
        {
            inputFunctions = new InputFunctions();

            mCursor = new Sprite();

            startColor = Color.White;
            endColor = Color.White;
            size = 0.8f;

            inputFunctions.onActionPressed = onActionPressed;
            inputFunctions.onMove = onMove;
            mBgColor = Color.Black;

            effect = new EffectManager();

            currentSelection = ParticleEditorOptions.START_COLOR;
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("guiFont");

            Rectangle srcRect = new Rectangle(0, 0, 64, 21);
            mCursor.Create(content.Load<Texture2D>("rocket-64x21"), srcRect);

            scripts = content.Load<ScriptHolder[]>("languages");

            scripts[language].particleEditorScript.Refresh();
            options = scripts[language].particleEditorScript.GetItems();

            scripts[language].colorScript.Refresh();
            colorStrings = scripts[language].colorScript.GetItems();

            colors = new Color[colorStrings.Length];

            currentStartColor = 0;
            currentEndColor = 0;

            SetUpColorArray();

            changiningInformation = new string[options.Length - 1];

            DetermineDerivedAttributes();
        }

        public void SetCenter(Vector2 center)
        {
            mScreenCenter = center;
        }

        public ParticleEditorOptions GetCurrentSelection()
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

            scripts[language].particleEditorScript.Refresh();
            options = scripts[language].particleEditorScript.GetItems();

            changiningInformation[0] = colorStrings[currentStartColor];
            changiningInformation[1] = colorStrings[currentEndColor];
            changiningInformation[2] = size.ToString();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Vector2 pos = Vector2.Zero;

                pos.X = mTextPosition.X;
                pos.Y = mTextPosition.Y + (widestItemDimensions.Y * i + MARGIN);

                spriteBatch.DrawString(font, options[i], pos, fontColor);

                if (i != options.Length - 1)
                    spriteBatch.DrawString(font, changiningInformation[i], new Vector2(pos.X + widestItemDimensions.X + 50, pos.Y), Color.White);

            }

            mCursor.DrawAtCenter(spriteBatch);

            base.Draw(spriteBatch);
        }

        // Private Methods ///////////////////////////////////////////////////////////////

        private void DetermineDerivedAttributes()
        {
            for (int i = 0; i < options.Length; i++)
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

            currentSelection = (ParticleEditorOptions)MathHelper.Clamp((float)currentSelection, (float)ParticleEditorOptions.START + 1, (float)ParticleEditorOptions.NUM_OPTIONS - 1);
        }

        private void IncreaseCurrentSelection()
        {
            switch (currentSelection)
            {
                case ParticleEditorOptions.SIZE:
                    {
                        size += 0.05f;
                        size = MathHelper.Clamp(size, 0.01f, 5.0f);
                    }
                    break;
                case ParticleEditorOptions.START_COLOR:
                    {
                        currentStartColor++;
                        currentStartColor = currentStartColor % colorStrings.Length;
                    }
                    break;
                case ParticleEditorOptions.END_COLOR:
                    {
                        currentEndColor++;
                        currentEndColor = currentEndColor % colorStrings.Length;
                    }
                    break;
                default:
                    break;
            }
        }

        private void DecreaseCurrentSelection()
        {
            switch (currentSelection)
            {
                case ParticleEditorOptions.SIZE:
                    {
                        size -= 0.05f;
                        size = MathHelper.Clamp(size, 0.01f, 5.0f);
                    }
                    break;
                case ParticleEditorOptions.START_COLOR:
                    {
                        currentStartColor--;
                        currentStartColor = (currentStartColor + colorStrings.Length) % colorStrings.Length;
                    }
                    break;
                case ParticleEditorOptions.END_COLOR:
                    {
                        currentEndColor--;
                        currentEndColor = (currentEndColor + colorStrings.Length) % colorStrings.Length;
                    }
                    break;
                default:
                    break;
            }
        }

        private void SetUpColorArray()
        {
            colors[0] = Color.White;
            colors[1] = Color.Black;
            colors[2] = Color.Gray;
            colors[3] = Color.DarkGray;
            colors[4] = Color.LightGray;
            colors[5] = Color.Yellow;
            colors[6] = Color.Blue;
        }

        // Input Methods /////////////////////////////////////////////////////////////////

        public void onActionPressed()
        {
            if (currentSelection == ParticleEditorOptions.QUIT)
            {
                CustomParticle.SetAttributes(size, startColor, endColor);

                mScreenEvent.Invoke(this, EventArgs.Empty);
            }
        }

        public void onMove(Vector2 direction)
        {
            Vector2 newDirection = InputListener.SnapToOneDirection(direction);

            if (analogStickIsNeutral)
            {
                if (newDirection.Y != 0 || newDirection.X != 0)
                {
                    analogStickIsNeutral = false;

                    if (newDirection.Y == 1)
                    {
                        ChangeCurrentSelection(1);
                    }
                    else if (newDirection.Y == -1)
                    {
                        ChangeCurrentSelection(-1);
                    }
                    else if (newDirection.X == 1)
                    {
                        IncreaseCurrentSelection();
                    }
                    else if (newDirection.X == -1)
                    {
                        DecreaseCurrentSelection();
                    }
                }
            }

            if (newDirection.Y == 0 && newDirection.X == 0)
                analogStickIsNeutral = true;
        }
    }
}
