using DiagramLibrary.Text;
using Rhino.Geometry;

namespace DiagramLibrary
{
    public class DiagramTextJustification : IDiagramTextJustification
    {
        public TextJustification Justification { get; }

        public bool IsCenter { get; }

        public bool IsRight { get; }

        public bool IsTop { get; }

        public bool IsMiddle { get; }

        public DiagramTextJustification(TextJustification justification)
        {
            this.Justification = justification;

            this.IsCenter = this.Justification == TextJustification.Center ||
                            this.Justification == TextJustification.BottomCenter ||
                            this.Justification == TextJustification.MiddleCenter ||
                            this.Justification == TextJustification.TopCenter;

            this.IsRight = this.Justification == TextJustification.Right ||
                           this.Justification == TextJustification.BottomRight ||
                           this.Justification == TextJustification.MiddleRight ||
                           this.Justification == TextJustification.TopRight;

            this.IsMiddle = this.Justification == TextJustification.Middle ||
                            this.Justification == TextJustification.MiddleLeft ||
                            this.Justification == TextJustification.MiddleCenter ||
                            this.Justification == TextJustification.MiddleRight;

            this.IsTop = this.Justification == TextJustification.Top ||
                         this.Justification == TextJustification.TopLeft ||
                         this.Justification == TextJustification.TopCenter ||
                         this.Justification == TextJustification.TopRight;

        }
    }
}