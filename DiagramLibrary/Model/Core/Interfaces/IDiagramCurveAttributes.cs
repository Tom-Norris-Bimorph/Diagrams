using System.Drawing;

namespace DiagramLibrary.Core
{
    public interface IDiagramCurveAttributes
    {
        Color Colour { get; }
        float LineWeight { get; }
        Pen GetPen();
        Brush GetBrush();
    }
}