using DiagramLibrary.Core;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramCurveAttributes : IDiagramCurveAttributes
    {
        public Color Colour { get; }

        public float LineWeight { get; }

        public DiagramCurveAttributes(Color color, float lineWeight)
        {
            this.Colour = color;
            this.LineWeight = lineWeight;
        }

        public Pen GetPen()
        {
            return new Pen(this.Colour, this.LineWeight);
        }

        public Brush GetBrush()
        {
            return new SolidBrush(this.Colour);
        }
    }
}