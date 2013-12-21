using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public class ColorScript
    {
        public string White;
        public string Black;
        public string Gray;
        public string DarkGray;
        public string LightGray;
        public string Yellow;
        public string Blue;

        private string[] options;

        public ColorScript()
        {
            options = new string[7];
        }

        public void Refresh()
        {
            options[0] = White;
            options[1] = Black;
            options[2] = Gray;
            options[3] = DarkGray;
            options[4] = LightGray;
            options[5] = Yellow;
            options[6] = Blue;
        }

        public string[] GetItems()
        {
            return options;
        }
    }
}
