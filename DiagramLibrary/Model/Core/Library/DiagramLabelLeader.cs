using DiagramLibrary.Core;
using System.Drawing;

namespace DiagramLibrary
{

    public class DiagramLabelLeader : IDiagramLabelLeader
    {
        public PointF LeaderLocation { get; }
        public float Offset { get; }

        public IDiagramCurve LeaderCurve { get; }

        public DiagramLabelLeader(IDiagramCurve leaderCurve, PointF leaderLocation, float offset)
        {
            this.LeaderLocation = leaderLocation;
            this.Offset = offset;
            this.LeaderCurve = leaderCurve;
        }

        public IDiagramObject Duplicate()
        {
            var curveCopy = this.LeaderCurve.DuplicateDiagramCurve();

            return new DiagramLabelLeader(curveCopy, this.LeaderLocation, this.Offset);
        }
    }
}