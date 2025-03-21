using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary
{

    public class DiagramLocation : IDiagramLocation
    {
        public PointF Point { get; }

        public DiagramLocation(PointF location)
        {
            this.Point = location;
        }

        public DiagramLocation(Point3d location)
        {
            this.Point = DiagramCoordinateSystem.ConvertPoint(location);
        }
    }
}