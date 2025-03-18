using DiagramLibrary.Core;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramAttributes : IDiagramAttributes
    {
        public Color Colour { get; }

        public float LineWeight { get; }

        public DiagramAttributes(Color color, float lineWeight)
        {
            this.Colour = color;
            this.LineWeight = lineWeight;
        }

        public Pen GetPen()
        {
            return new Pen(this.Colour, this.LineWeight);
        }
    }
}