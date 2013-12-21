using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public class ScriptHolder
    {
        public string LanguageName;
        public MainMenuScript mainMenuScript;
        public PauseMenuScript pauseMenuScript;
        public ParticleEditorScript particleEditorScript;
        public ColorScript colorScript;

        public ScriptHolder()
        {
            // Empty
        }
    }
}
