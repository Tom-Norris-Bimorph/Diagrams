using DiagramLibrary.Core;
using Rhino.Geometry;

namespace DiagramLibrary

{
    public interface IBaseCurveDiagramObject : IDiagramObject
    {
        IBaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation);
    }
}