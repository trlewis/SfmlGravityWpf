using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlGravityWpf.Code.Helpers
{
    using SFML.Graphics;

    public static class ColorHelper
    {
        public static Color GetRandomColor()
        {
            var rand = new Random();
            return new Color((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
        }
    }
}
