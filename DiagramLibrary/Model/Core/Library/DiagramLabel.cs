using DiagramLibrary.Core;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramLabel : IDrawableDiagramObject
    {

        private PointF m_LeaderLocation;
        private float m_Offset;
        private DiagramCurve m_leader;

        public DiagramText DiagramText { get; }

        public static DiagramLabel Create(string text, PointF leaderLocation, float offset, Vector3d direction, Color colour, float lineWeight, float textSize,
          Color maskColour, Color frameColor, float frameLineWeight,
          string fontName, float padding, DiagramCurveEnd crvEnd)
        {

            var diagramLabel = new DiagramLabel();

            var line = new Line(Diagram.ConvertPoint(leaderLocation), direction, offset);
            var directionUnitized = direction;
            directionUnitized.Unitize();
            var justification = Rhino.Geometry.TextJustification.None;
            var underlineDirection = new Vector3d(1, 0, 0);

            if (directionUnitized.X == 0)
            {
                justification = TextJustification.BottomCenter;
            }

            if (directionUnitized.X < 0)
            {
                justification = TextJustification.BottomRight;
                underlineDirection = new Vector3d(-1, 0, 0);
            }

            if (directionUnitized.X > 0)
            {
                justification = TextJustification.BottomLeft;
            }

            diagramLabel.m_DiagramText = DiagramText.Create(text, Diagram.ConvertPoint(line.To), colour, textSize, justification, maskColour, frameColor, frameLineWeight, fontName, new SizeF(-1, -1), padding, Rhino.Geometry.TextJustification.BottomLeft);
            var line2 = new Line(line.To, underlineDirection, diagramLabel.m_DiagramText.GetTotalSize().Width);
            diagramLabel.m_leader = DiagramCurve.Create(new Polyline(new Point3d[] { line.From, line.To, line2.To }).ToNurbsCurve(), colour, lineWeight);

            if (crvEnd != null)
            {
                diagramLabel.m_leader.AddCurveEnds(crvEnd, null);
            }
            diagramLabel.m_LeaderLocation = leaderLocation;
            diagramLabel.m_Offset = offset;

            return diagramLabel;
        }

        public override DiagramObject Duplicate()
        {
            var diagramLabel = new DiagramLabel();
            diagramLabel.m_DiagramText = m_DiagramText.Duplicate() as DiagramText;
            diagramLabel.m_LeaderLocation = m_LeaderLocation;
            diagramLabel.m_Offset = m_Offset;
            diagramLabel.m_leader = m_leader;
            return diagramLabel;
        }

        public override BoundingBox GetBoundingBox()
        {
            var bb = new BoundingBox(new Point3d[] { Diagram.ConvertPoint(m_LeaderLocation) });
            bb.Union(m_DiagramText.GetBoundingBox());

            return bb;
        }

        public override void DrawBitmap(Graphics g)
        {
            m_leader.DrawBitmap(g);
            m_DiagramText.DrawBitmap(g);
        }

        public override void DrawRhinoPreview(DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
            m_leader.DrawRhinoPreview(pipeline, tolerance, xform, state);
            m_DiagramText.DrawRhinoPreview(pipeline, tolerance, xform, state);

            return;
        }

        public override List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();
            outList.AddRange(m_leader.BakeRhinoPreview(tolerance, xform, state, doc, attr));
            outList.AddRange(m_DiagramText.BakeRhinoPreview(tolerance, xform, state, doc, attr));

            return outList;
        }
    }
}
