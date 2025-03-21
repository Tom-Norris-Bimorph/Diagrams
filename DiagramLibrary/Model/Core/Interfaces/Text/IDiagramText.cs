using DiagramLibrary.Core;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary.Text
{
    public interface IDiagramText : IDrawableDiagramObject
    {
        IDiagramTextAttributes TextAttributes { get; }

        IDiagramFilledRectangle Mask { get; }
        IDiagramLocation Location { get; }
        IDiagramCurveAttributes CurveAttributes { get; }

        string Text { get; }

        List<string> CalculateTextLines(Graphics g, Font font, SizeF maxSize, StringFormat format, out SizeF totalSize, out List<SizeF> rowSizes);

        SizeF CalculateTextSize(Graphics g, out SizeF maskSize, out List<string> lines, out List<SizeF> rowSizes);

    }
}