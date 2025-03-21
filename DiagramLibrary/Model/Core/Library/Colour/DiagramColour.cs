using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramColour
    {
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            //The ranges are 0 - 360 for hue, and 0 - 1 for saturation or value
            var hueInterval = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            var hueFractional = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            var valueInt = Convert.ToInt32(value);
            var p = Convert.ToInt32(value * (1 - saturation));
            var q = Convert.ToInt32(value * (1 - hueFractional * saturation));
            var t = Convert.ToInt32(value * (1 - (1 - hueFractional) * saturation));

            switch (hueInterval)
            {
                case 0:
                    return Color.FromArgb(255, valueInt, t, p);
                case 1:
                    return Color.FromArgb(255, q, valueInt, p);
                case 2:
                    return Color.FromArgb(255, p, valueInt, t);
                case 3:
                    return Color.FromArgb(255, p, q, valueInt);
                case 4:
                    return Color.FromArgb(255, t, p, valueInt);
                default:
                    return Color.FromArgb(255, valueInt, p, q);
            }
        }

        public static List<Color> GetColors(int count)
        {
            //To Do support starting colourss
            var clrs = new List<Color>();
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < count; i++)
            {
                var h = rand.NextDouble() * 360;
                var s = rand.NextDouble();
                var v = rand.NextDouble();

                clrs.Add(DiagramColour.ColorFromHSV(h, s, v));
            }

            return clrs;

        }

        public static List<Color> GetColors(int count, Color startingColour)
        {
            var clrs = new List<Color>();
            for (var i = 0; i < count; i++)
            {

                clrs.Add(startingColour);
            }
            return clrs;
        }
    }
}
