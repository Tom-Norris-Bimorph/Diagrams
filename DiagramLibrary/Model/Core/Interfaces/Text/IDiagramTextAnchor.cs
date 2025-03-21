using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary.Text
{
    public interface IDiagramTextAnchor
    {
        TextJustification Anchor { get; }
        PointF GetAnchorCompensatedPoint(IDiagramLocation diagramLocation, SizeF size, float textHeight);
    }
}