using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Themes
{
    class ThemeDirector
    {
        public ThemeBuilder ThemeBuilder;

        public void SetThemeBuilder(ThemeBuilder tb)
        {
            ThemeBuilder = tb;
        }

        public Theme GeTheme()
        {
            return ThemeBuilder.GetTheme();
        }

        public void ConstructTheme()
        {
            ThemeBuilder.CreateNewTheme();
            ThemeBuilder.BuildTextColor();
            ThemeBuilder.BuildBackgroundColor();
            ThemeBuilder.BuildFontSize();
        }
    }
}
