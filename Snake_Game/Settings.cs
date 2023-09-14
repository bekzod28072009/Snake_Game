using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Game
{
    class Settings
    {
        public static int Width { get; set; }

        public static int Heigth { get; set;}

        public static string direction;

        public Settings()
        {
            Width = 16;
            Heigth = 16;
            direction = "left";
        }
    }
}
