using DiagramLibrary.Core;
using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramCoordinateSystem : IDiagramCoordinateSystem
    {
        public IDiagramLocation Location { get; }

        public DiagramCoordinateSystem(IDiagramLocation location)
        {
            this.Location = location;
        }
        public static PointF ConvertPoint(Point3d pt)
        {
            return new PointF((float)pt.X, (float)pt.Y);
        }

        public static Point3d ConvertPoint(PointF pt)
        {
            return new Point3d(pt.X, pt.Y, 0);
        }
    }
}
