using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary.Core
{
    public interface IDiagramCoordinateSystem
    {
        PointF Location { get; }
        PointF ConvertPoint(Point3d pt);
        Point3d ConvertPoint(PointF pt);
    }
}