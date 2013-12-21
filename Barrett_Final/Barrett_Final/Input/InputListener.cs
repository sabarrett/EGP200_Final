using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SciFiDemo.Input
{
    class InputListener
    {
        public delegate void digitalInputDelegate();
        public delegate void analogInputDelegate(Vector2 directionValue);

        public digitalInputDelegate onActionPressed;
        public digitalInputDelegate onBackPressed;
        public digitalInputDelegate onSpecial1Pressed;
        public digitalInputDelegate onSpecial2Pressed;
        public digitalInputDelegate onPausePressed;
        public digitalInputDelegate onSelectPressed;
        public digitalInputDelegate onR1Pressed;
        public digitalInputDelegate onR2Pressed;
        public digitalInputDelegate onL1Pressed;
        public digitalInputDelegate onL2Pressed;
        public digitalInputDelegate onRightPressed;
        public digitalInputDelegate onLeftPressed;
        public digitalInputDelegate onUpPressed;
        public digitalInputDelegate onDownPressed;
        
        public analogInputDelegate onMove;

        public Buttons actionButton = Buttons.A;
        public Keys actionKey = Keys.Space;

        public Buttons backButton = Buttons.B;
        public Keys backKey = Keys.Escape;

        public Buttons special1Button = Buttons.X;
        public Keys special1Key = Keys.F;

        public Buttons special2Button = Buttons.Y;
        public Keys special2Key = Keys.D;

        public Buttons pauseButton = Buttons.Start;
        public Keys pauseKey = Keys.Enter;

        public Buttons selectButton = Buttons.Back;
        public Keys selectKey = Keys.P;

        public Buttons r1Button = Buttons.RightShoulder;
        public Keys r1Key = Keys.R;

        public Buttons r2Button = Buttons.RightTrigger;
        public Keys r2Key = Keys.E;

        public Buttons l1Button = Buttons.LeftShoulder;
        public Keys l1Key = Keys.W;

        public Buttons l2Button = Buttons.LeftTrigger;
        public Keys l2Key = Keys.Q;

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        GamePadState[] oldGamePadStates;
        GamePadState[] newGamePadStates;

        GamePadState oldGamePadState;
        GamePadState newGamePadState;

        PlayerIndex firstPlayer = PlayerIndex.One;

        public InputListener()
        {
            oldKeyboardState = new KeyboardState();
            newKeyboardState = new KeyboardState();
            oldGamePadState = new GamePadState();
            newGamePadState = new GamePadState();

            oldGamePadStates = new GamePadState[4];
            newGamePadStates = new GamePadState[4];

            Initialize();
        }

        public void Initialize()
        {
            onActionPressed   =   DigitalDoNothing;
            onBackPressed     =   DigitalDoNothing;
            onSpecial1Pressed =   DigitalDoNothing;
            onSpecial2Pressed =   DigitalDoNothing;
            onPausePressed    =   DigitalDoNothing;
            onSelectPressed   =   DigitalDoNothing;
            onR1Pressed       =   DigitalDoNothing;
            onR2Pressed       =   DigitalDoNothing;
            onL1Pressed       =   DigitalDoNothing;
            onL2Pressed       =   DigitalDoNothing;
            onRightPressed    =   DigitalDoNothing;
            onLeftPressed     =   DigitalDoNothing;
            onUpPressed       =   DigitalDoNothing;
            onDownPressed     =   DigitalDoNothing;

            onMove            =   AnalogDoNothing;
        }

        public void Update()
        {
            getNewStates();
            CheckAndCallAllInput();
            setOldStatesToNewStates();
        }

        public void SetFirstPlayer(PlayerIndex index)
        {
            firstPlayer = index;
        }

        private void getNewStates()
        {
            newKeyboardState = Keyboard.GetState();
            newGamePadState = GamePad.GetState(firstPlayer);
        }

        private void setOldStatesToNewStates()
        {
            oldGamePadState = newGamePadState;
            oldKeyboardState = newKeyboardState;
        }

        private bool WasButtonPressedThisFrame(Buttons button)
        {
            if (!oldGamePadState.IsButtonDown(button) &&
                    newGamePadState.IsButtonDown(button))
                return true;
            else
                return false;
        }

        private bool WasKeyPressedThisFrame(Keys key)
        {
            if (!oldKeyboardState.IsKeyDown(key) &&
                    newKeyboardState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        private bool CheckInput(Keys key, Buttons button)
        {
            if (WasKeyPressedThisFrame(key) || WasButtonPressedThisFrame(button))
            {
                return true;
            }

            return false;
        }

        private void CheckAndCallAllInput()
        {
            if (CheckInput(actionKey, actionButton))
                onActionPressed();

            if (CheckInput(backKey, backButton))
                onBackPressed();

            if (CheckInput(special1Key, special1Button))
                onSpecial1Pressed();

            if (CheckInput(special2Key, special2Button))
                onSpecial2Pressed();

            if (CheckInput(pauseKey, pauseButton))
                onPausePressed();

            if (CheckInput(selectKey, selectButton))
                onSelectPressed();

            if (CheckInput(r1Key, r1Button))
                onR1Pressed();

            if (CheckInput(r2Key, r2Button))
                onR2Pressed();

            if (CheckInput(l1Key, l2Button))
                onL1Pressed();

            if (CheckInput(l2Key, l2Button))
                onL2Pressed();
            
            // Deal with movement /////////////////////////////////////////////////
            Vector2 digitalInput = new Vector2();

            if (CheckInput(Keys.Left, Buttons.DPadLeft))
            {
                onLeftPressed();
            }

            if (CheckInput(Keys.Right, Buttons.DPadRight))
            {
                onRightPressed();
            }

            if (CheckInput(Keys.Up, Buttons.DPadUp))
            {
                onUpPressed();
            }

            if (CheckInput(Keys.Down, Buttons.DPadDown))
            {
                onDownPressed();
            }

            if (newKeyboardState.IsKeyDown(Keys.Left) || newGamePadState.IsButtonDown(Buttons.DPadLeft))
                digitalInput.X -= 1;
            if (newKeyboardState.IsKeyDown(Keys.Right) || newGamePadState.IsButtonDown(Buttons.DPadRight))
                digitalInput.X += 1;
            if (newKeyboardState.IsKeyDown(Keys.Up) || newGamePadState.IsButtonDown(Buttons.DPadUp))
                digitalInput.Y -= 1;
            if (newKeyboardState.IsKeyDown(Keys.Down) || newGamePadState.IsButtonDown(Buttons.DPadDown))
                digitalInput.Y += 1;

            Vector2 analogInput = newGamePadState.ThumbSticks.Left;
            analogInput.Y = -analogInput.Y;

            if (analogInput.X != 0 || analogInput.Y != 0)
            {
                onMove(analogInput);
            }
            else
            {
                onMove(digitalInput);
            }
        }

        public void AttachFunctions(InputFunctions functions)
        {
            onActionPressed += functions.onActionPressed;
            onBackPressed += functions.onBackPressed;
            onSpecial1Pressed += functions.onSpecial1Pressed;
            onSpecial2Pressed += functions.onSpecial2Pressed;
            onPausePressed += functions.onPausePressed;
            onSelectPressed += functions.onSelectPressed;
            onR1Pressed += functions.onR1Pressed;
            onR2Pressed += functions.onR2Pressed;
            onL1Pressed += functions.onL1Pressed;
            onL2Pressed += functions.onL2Pressed;
            onRightPressed += functions.onRightPressed;
            onLeftPressed += functions.onLeftPressed;
            onUpPressed += functions.onUpPressed;
            onDownPressed += functions.onDownPressed;

            onMove += functions.onMove;
        }

        public void DetachFunctions(InputFunctions functions)
        {
            onActionPressed -= functions.onActionPressed;
            onBackPressed -= functions.onBackPressed;
            onSpecial1Pressed -= functions.onSpecial1Pressed;
            onSpecial2Pressed -= functions.onSpecial2Pressed;
            onPausePressed -= functions.onPausePressed;
            onSelectPressed -= functions.onSelectPressed;
            onR1Pressed -= functions.onR1Pressed;
            onR2Pressed -= functions.onR2Pressed;
            onL1Pressed -= functions.onL1Pressed;
            onL2Pressed -= functions.onL2Pressed;
            onRightPressed -= functions.onRightPressed;
            onLeftPressed -= functions.onLeftPressed;
            onUpPressed -= functions.onUpPressed;
            onDownPressed -= functions.onDownPressed;

            onMove -= functions.onMove;
        }

        public void DigitalDoNothing()
        {

        }

        public void AnalogDoNothing(Vector2 zero)
        {

        }

        public static Vector2 SnapToOneDirection(Vector2 origDirection)
        {
            Vector2 modifiedDirection = new Vector2();

            if (origDirection.Y > -0.5f && origDirection.Y < 0.5f)
            {
                if (origDirection.X > 0.15f)
                {
                    modifiedDirection = new Vector2(1, 0);
                }
                else if (origDirection.X < -0.15f)
                {
                    modifiedDirection = new Vector2(-1, 0);
                }
                else
                {
                    modifiedDirection = new Vector2(0, 0);
                }
            }
            else if (origDirection.X > -0.5f && origDirection.X < 0.5f)
            {
                if (origDirection.Y > 0.15f)
                {
                    modifiedDirection = new Vector2(0, 1);
                }
                else if (origDirection.Y < -0.15f)
                {
                    modifiedDirection = new Vector2(0, -1);
                }
                else
                {
                    modifiedDirection = new Vector2(0, 0);
                }
            }

            return modifiedDirection;
        }

    }

    
}
