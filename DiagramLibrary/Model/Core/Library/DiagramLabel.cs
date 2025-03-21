using DiagramLibrary.Core;
using DiagramLibrary.Text;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{

    public class DiagramLabel : IDiagramLabel
    {
        public IDiagramCurveAttributes CurveAttributes { get; }
        public IDiagramLocation Location { get; }
        public IDiagramLabelLeader Leader { get; }
        public IDiagramText DiagramText { get; }

        public DiagramLabel(IDiagramText diagramText, IDiagramLabelLeader leader = null)
        {
            this.DiagramText = diagramText;
            this.Leader = leader;
        }

        /* public DiagramLabel(string text, PointF leaderLocation, float offset, Vector3d direction, Color colour, float lineWeight, float textSize,
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

             diagramLabel.this.DiagramText = this.DiagramText.Create(text, Diagram.ConvertPoint(line.To), colour, textSize, justification, maskColour, frameColor, frameLineWeight, fontName, new SizeF(-1, -1), padding, Rhino.Geometry.TextJustification.BottomLeft);
             var line2 = new Line(line.To, underlineDirection, diagramLabel.this.DiagramText.GetTotalSize().Width);
             diagramLabel.Leader = DiagramCurve.Create(new Polyline(new Point3d[] { line.From, line.To, line2.To }).ToNurbsCurve(), colour, lineWeight);

             if (crvEnd != null)
             {
                 diagramLabel.Leader.AddCurveEnds(crvEnd, null);
             }
             diagramLabel.LeaderLocation = leaderLocation;
             diagramLabel.Offset = offset;

             return diagramLabel;
         }*/

        public IDiagramObject Duplicate()
        {
            var textCopy = this.DiagramText.Duplicate() as DiagramText;
            var leaderCopy = this.Leader.Duplicate() as DiagramLabelLeader;

            return new DiagramLabel(textCopy, leaderCopy);
        }

        public PointF GetBoundingBoxLocation()
        {
            var boundingBox = this.GetBoundingBox();
            return DiagramCoordinateSystem.ConvertPoint(boundingBox.Min);
        }

        public SizeF GetTotalSize()
        {
            var boundingBox = this.GetBoundingBox();
            return new SizeF((float)(boundingBox.Max.X - boundingBox.Min.X), (float)(boundingBox.Max.Y - boundingBox.Min.Y));
        }

        public BoundingBox GetBoundingBox()
        {
            var boundingBox = new BoundingBox(new Point3d[] { DiagramCoordinateSystem.ConvertPoint(this.Leader.LeaderLocation) });
            boundingBox.Union(this.DiagramText.GetBoundingBox());

            return boundingBox;
        }

        public void DrawBitmap(Graphics g)
        {
            this.Leader.LeaderCurve.DrawBitmap(g);
            this.DiagramText.DrawBitmap(g);
        }

        public void DrawRhinoPreview(DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
            this.Leader.LeaderCurve.DrawRhinoPreview(pipeline, tolerance, xform, state);
            this.DiagramText.DrawRhinoPreview(pipeline, tolerance, xform, state);

            return;
        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();
            outList.AddRange(this.Leader.LeaderCurve.BakeRhinoPreview(tolerance, transform, state, doc, attr));
            outList.AddRange(this.DiagramText.BakeRhinoPreview(tolerance, transform, state, doc, attr));

            return outList;
        }
    }
}
