using DiagramLibrary.Core;
using DiagramLibrary.Text;

namespace DiagramLibrary
{
    public interface IDiagramDimension : IDiagramObject
    {
        IDimensionAttributes DimensionAttributes { get; }
        IDiagramCurve Curve { get; }
        IDiagramText Text { get; }
        IDiagramCurveAttributes CurveAttributes { get; }
        IDiagramTextAttributes TextAttributes { get; }
    }
}