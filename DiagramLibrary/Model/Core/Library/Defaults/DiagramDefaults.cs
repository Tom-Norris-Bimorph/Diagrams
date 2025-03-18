using DiagramLibrary.Core;
using System.Drawing;

namespace DiagramLibrary.Defaults
{
    public class DiagramDefaults
    {
        public static Color DefaultColor = Color.Black;
        public static float DefaultLineWeight = 1f;
        public static float DefaultTextScale = 10f;
        public static string DefaultFontName = "Arial";
        public static float DefaultPadding = 3f;
        public static Color SelectedColor = Color.FromArgb(128, Color.ForestGreen);
        public static IDiagramAttributes DefaultDiagramAttributes = new DiagramAttributes(DefaultColor, DefaultLineWeight);
    }
}