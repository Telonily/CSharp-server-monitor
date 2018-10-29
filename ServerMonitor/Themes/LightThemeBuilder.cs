using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ServerMonitor.Themes
{
    class LightThemeBuilder : ThemeBuilder
    {
        public override void BuildTextColor()
        {
            Theme.TextColor = Color.FromRgb(20, 20, 20);
        }

        public override void BuildBackgroundColor()
        {
            Theme.BackgroundColor = Color.FromRgb(220, 230, 230);
        }

        public override void BuildFontSize()
        {
            Theme.FontSize = 12;
        }
    }
}
