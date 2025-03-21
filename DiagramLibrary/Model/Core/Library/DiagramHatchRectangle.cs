using DiagramLibrary.Core;
using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramHatchRectangle : DiagramHatch
    {
        public DiagramHatchRectangle(Rectangle3d outerRectangle, IDiagramCurveAttributes diagramCurveAttributes,
            IDiagramHatchAttributes hatchAttributes) : base(outerRectangle.ToNurbsCurve(), diagramCurveAttributes, hatchAttributes)
        { }

        public DiagramHatchRectangle(PointF origin, SizeF size, IDiagramCurveAttributes diagramCurveAttributes,
            IDiagramHatchAttributes hatchAttributes) : base(new Rectangle3d(Plane.WorldXY, DiagramCoordinateSystem.ConvertPoint(origin),
            new Point3d(origin.X + size.Width, origin.Y + size.Height, 0)).ToNurbsCurve(), diagramCurveAttributes, hatchAttributes)
        { }
    }
}
