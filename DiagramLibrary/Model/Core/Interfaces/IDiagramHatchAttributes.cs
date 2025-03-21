using System.Drawing;

namespace DiagramLibrary
{
    public interface IDiagramHatchAttributes
    {
        System.Drawing.Drawing2D.HatchStyle HatchStyle { get; }
        bool IsSolid { get; }
        double HatchRotation { get; }
        double HatchScale { get; }
        Color BackColour { get; }
        Color Colour { get; }
        Brush GetBrush();
    }
}