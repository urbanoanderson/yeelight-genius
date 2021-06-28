using System;
using System.Windows.Media;

namespace YeelightGenius
{
    public static class Constants
    {
        public static TimeSpan LIGHT_DELAY = TimeSpan.FromSeconds(0.25);

        public static Color WHITE = Color.FromRgb(255, 255, 255);

        public static Color RED = Color.FromRgb(255, 0, 0);

        public static Color YELLOW = Color.FromRgb(255, 255, 0);

        public static Color GREEN = Color.FromRgb(0, 255, 0);

        public static Color BLUE = Color.FromRgb(0, 0, 255);

        public const string PATH_FX_DO = "./assets/do.wav";

        public const string PATH_FX_RE = "./assets/re.wav";

        public const string PATH_FX_MI = "./assets/mi.wav";

        public const string PATH_FX_FA = "./assets/fa.wav";
    }
}
