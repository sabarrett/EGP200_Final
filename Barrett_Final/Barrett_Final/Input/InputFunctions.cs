using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciFiDemo.Input
{
    class InputFunctions
    {
        public InputListener.digitalInputDelegate onActionPressed;
        public InputListener.digitalInputDelegate onBackPressed;
        public InputListener.digitalInputDelegate onSpecial1Pressed;
        public InputListener.digitalInputDelegate onSpecial2Pressed;
        public InputListener.digitalInputDelegate onPausePressed;
        public InputListener.digitalInputDelegate onSelectPressed;
        public InputListener.digitalInputDelegate onR1Pressed;
        public InputListener.digitalInputDelegate onR2Pressed;
        public InputListener.digitalInputDelegate onL1Pressed;
        public InputListener.digitalInputDelegate onL2Pressed;
        public InputListener.digitalInputDelegate onRightPressed;
        public InputListener.digitalInputDelegate onLeftPressed;
        public InputListener.digitalInputDelegate onUpPressed;
        public InputListener.digitalInputDelegate onDownPressed;

        public InputListener.analogInputDelegate onMove;

        public InputFunctions()
        {

        }
    }
}
