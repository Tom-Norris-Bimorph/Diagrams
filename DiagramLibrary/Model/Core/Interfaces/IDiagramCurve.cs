using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary
{
    public interface IDiagramCurve : IBaseCurveDiagramObject
    {
        Curve Curve { get; }
        IDiagramCurveEnd StartCurveEnd { get; }
        IDiagramCurveEnd EndCurveEnd { get; }

        IDiagramCurve DuplicateDiagramCurve();

        PointF[] GetPoints();
    }
}