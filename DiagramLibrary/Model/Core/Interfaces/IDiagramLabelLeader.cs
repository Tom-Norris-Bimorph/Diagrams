using DiagramLibrary.Core;
using System.Drawing;

namespace DiagramLibrary
{
    public interface IDiagramLabelLeader : IDiagramObject
    {
        PointF LeaderLocation { get; }
        float Offset { get; }
        IDiagramCurve LeaderCurve { get; }
    }
}