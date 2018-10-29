using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Themes
{
    abstract class ThemeBuilder
    {
        protected Theme Theme;

        public Theme GetTheme()
        {
            return Theme;
        }

        public void CreateNewTheme()
        {
            Theme = new Theme();
        }

        public abstract void BuildTextColor();
        public abstract void BuildBackgroundColor();
        public abstract void BuildFontSize();
    }
}
