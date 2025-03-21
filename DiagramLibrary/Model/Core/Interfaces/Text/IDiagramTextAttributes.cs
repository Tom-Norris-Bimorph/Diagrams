using DiagramLibrary.Core;
using System.Drawing;

namespace DiagramLibrary.Text
{
    public interface IDiagramTextAttributes
    {
        IDiagramCurveAttributes CurveAttributes { get; }

        IDiagramTextJustification Justification { get; }

        IDiagramTextAnchor Anchor { get; }
        SizeF WrapSize { get; }
        string FontName { get; }
        float TextSize { get; }

        float Padding { get; }

        new System.Drawing.Font Font { get; }
    }
}