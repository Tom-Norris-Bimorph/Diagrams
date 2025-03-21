using DiagramLibrary.Defaults;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramHatchAttributes : IDiagramHatchAttributes
    {

        public System.Drawing.Drawing2D.HatchStyle HatchStyle { get; }
        public bool IsSolid { get; } = true;

        public double HatchRotation { get; } = 0;

        public double HatchScale { get; } = 1;

        public Color BackColour { get; }

        public Color Colour { get; }

        public DiagramHatchAttributes(Color Colour = DiagramDefaults.DefaultHatchStyle,
            Color BackColour = DiagramDefaults.DefaultHatchStyle,
            System.Drawing.Drawing2D.HatchStyle hatchStyle = DiagramDefaults.DefaultHatchStyle,
            double hatchRotation = DiagramDefaults.DefaultHatchRotation,
            double hatchScale = DiagramDefaults.DefaultHatchScale)
        {
            this.Colour = Colour;
            this.BackColour = BackColour;
            this.HatchStyle = hatchStyle;
            this.IsSolid = isSolid;
            this.HatchRotation = hatchRotation;
            this.HatchScale = hatchScale;
        }

        public Brush GetBrush()
        {
            if (this.IsSolid)
            {
                return new SolidBrush(this.Colour);
            }
            else
            {
                return new System.Drawing.Drawing2D.HatchBrush(this.HatchStyle,
                    this.Colour, this.BackColour);
            }
        }
    }
}