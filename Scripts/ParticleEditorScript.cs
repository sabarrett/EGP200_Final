using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public class ParticleEditorScript
    {
        public string StartSize;
        public string EndSize;
        public string Color;
        public string Exit;

        private string[] options;

        public ParticleEditorScript()
        {
            options = new string[4];
        }

        public void Refresh()
        {
            options[0] = StartSize;
            options[1] = EndSize;
            options[2] = Color;
            options[3] = Exit;
        }

        public string[] GetItems()
        {
            return options;
        }
    }
}
