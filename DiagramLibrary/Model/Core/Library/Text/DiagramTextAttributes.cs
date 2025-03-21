using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using DiagramLibrary.Text;
using System.Drawing;

namespace DiagramLibrary
{

    public class DiagramTextAttributes : IDiagramTextAttributes
    {
        public IDiagramCurveAttributes CurveAttributes { get; }

        public IDiagramTextJustification Justification { get; }

        public IDiagramTextAnchor Anchor { get; }

        public SizeF WrapSize { get; }
        Color TextColour { get; }

        Color TextBackgroundColour { get; }
        public string FontName { get; }
        public float TextSize { get; }

        public float Padding { get; }

        public System.Drawing.Font Font { get; }

        public DiagramTextAttributes(IDiagramCurveAttributes curveAttributes = null, IDiagramTextJustification justification = null,
            IDiagramTextAnchor anchor = null, SizeF wrapSize = new SizeF(), Color textColour = new Color(), Color textBackground = new Color(),
            string fontName = DiagramDefaults.DefaultFontName, float textSize = DiagramDefaults.DefaultTextHeight,
            float padding = DiagramDefaults.DefaultPadding)
        {
            this.CurveAttributes = curveAttributes ?? DiagramDefaults.DefaultDiagramAttributes;

            this.Justification =
                justification ?? DiagramDefaults.DefaultDiagramJustification;

            this.Anchor = anchor ?? DiagramDefaults.DefaultDiagramAnchor;

            this.WrapSize = wrapSize.IsEmpty ? DiagramDefaults.DefaultWrapSize : wrapSize;

            this.TextColour = textColour.IsEmpty ? DiagramDefaults.DefaultColor : textColour;

            this.TextBackgroundColour = textBackground.IsEmpty ? DiagramDefaults.DefaultTextBackgroundColor : textBackground;

            this.FontName = fontName;

            this.TextSize = textSize;

            this.Padding = padding;

            this.Font = new System.Drawing.Font(this.FontName, this.TextSize, GraphicsUnit.Pixel);
        }
    }
}