using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public class PauseMenuScript
    {
        public string Resume;
        public string Exit;

        private string[] items;

        public PauseMenuScript()
        {
            items = new string[2];
        }

        public void Refresh()
        {
            items[0] = Resume;
            items[1] = Exit;
        }

        public string[] GetItems()
        {
            return items;
        }
    }
}
