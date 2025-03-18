using DiagramLibrary.Core;
using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary
{

    public class DiagramCoordinateSystem : IDiagramCoordinateSystem
    {
        public PointF Location { get; }

        public DiagramCoordinateSystem(PointF location)
        {
            this.Location = location;
        }
        public PointF ConvertPoint(Point3d pt)
        {
            return new PointF((float)pt.X, (float)pt.Y);
        }

        public Point3d ConvertPoint(PointF pt)
        {
            return new Point3d(pt.X, pt.Y, 0);
        }
    }
}
