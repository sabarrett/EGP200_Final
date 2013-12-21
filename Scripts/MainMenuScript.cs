using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public class MainMenuScript
    {
        public string Play;
        public string ContentEditor;
        public string Language;
        public string Exit;
        public string Jump;
        public string Shoot;

        private string[] items;

        public MainMenuScript()
        {
            items = new string[4];
        }

        public void Refresh()
        {
            items[0] = Play;
            items[1] = ContentEditor;
            items[2] = Language;
            items[3] = Exit;
        }

        public string[] GetItems()
        {
            return items;
        }
    }
}
