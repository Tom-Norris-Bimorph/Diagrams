using DiagramLibrary.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramCurve : IBaseCurveDiagramObject
    {
        public Curve Curve { get; }

        public IDiagramCurveEnd StartCurveEnd { get; }

        public IDiagramCurveEnd EndCurveEnd { get; }

        public IDiagramAttributes Attributes { get; }

        public DiagramCurve(Curve curve, IDiagramAttributes diagramAttributes)
        {
            this.Curve = curve;
            this.Attributes = diagramAttributes;
            this.StartCurveEnd = DiagramCurveEnd.DefaultCurveEnd;
            this.EndCurveEnd = DiagramCurveEnd.DefaultCurveEnd;
        }

        public DiagramCurve DuplicateDiagramCurve()
        {
            return this.Duplicate() as DiagramCurve;
        }

        public IDiagramObject Duplicate()
        {
            var diagramCurve = new DiagramCurve();
            diagramCurve._colour = _colour;
            diagramCurve._lineWeight = _lineWeight;
            diagramCurve.Curve = this.Curve.DuplicateCurve();

            diagramCurve.StartCurveEnd = this.StartCurveEnd.DuplicateCurveEnd();

            diagramCurve.EndCurveEnd = this.EndCurveEnd.DuplicateCurveEnd();

            return diagramCurve;
        }

        public BoundingBox GetBoundingBox()
        {
            return this.Curve.GetBoundingBox(true);
        }

        public void AddCurveEnds(IBaseCurveDiagramObject start, Point3d setPointStart, Vector3d setDirectionStart,
            IBaseCurveDiagramObject end, Point3d setPointEnd, Vector3d setDirectionEnd)
        {
            if (start != DiagramCurveEnd.DefaultCurveEnd)
            {
                this.StartCurveEnd = new DiagramCurveEnd(start, setPointStart, setDirectionStart, true);

            }

            if (end != DiagramCurveEnd.DefaultCurveEnd)
            {
                this.EndCurveEnd = new DiagramCurveEnd(end, setPointEnd, setDirectionEnd, false);

            }
        }

        public void AddCurveEnds(DiagramCurveEnd start, DiagramCurveEnd end)
        {
            if (start != DiagramCurveEnd.DefaultCurveEnd)
            {
                this.StartCurveEnd = start;

            }

            if (end != DiagramCurveEnd.DefaultCurveEnd)
            {
                this.EndCurveEnd = end;

            }
        }

        public IBaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation)
        {
            if (baseDirection == Vector3d.Unset)
            {
                return null;
            }

            var clone = this.Duplicate() as DiagramCurve;

            clone.Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
            var angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);

            clone.Curve.Rotate(angle, Plane.WorldXY.Normal, location);

            return clone;
        }

        public void DrawBitmap(Graphics g)
        {

            if (this.StartCurveEnd != null)

            {
                this.StartCurveEnd.DrawBitmap(g, this.Curve.PointAtStart, this.Curve.TangentAtStart);
            }

            if (this.EndCurveEnd != null)
            {
                this.EndCurveEnd.DrawBitmap(g, this.Curve.PointAtEnd, this.Curve.TangentAtEnd);

            }

            var pts = this.GetPoints();
            if (pts != null)
            {
                g.DrawLines(this.GetPen(), pts);
            }
        }

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {

            var crv = this.GeneratePreviewGeometry(state, xform, out var clr, out var thickness);

            if (this.StartCurveEnd != null)

            {
                this.StartCurveEnd.DrawRhinoPreview(pipeline, tolerance, xform.Clone(), state, this.Curve.PointAtStart, this.Curve.TangentAtStart);
            }

            if (this.EndCurveEnd != null)
            {
                this.EndCurveEnd.DrawRhinoPreview(pipeline, tolerance, xform.Clone(), state, this.Curve.PointAtEnd, this.Curve.TangentAtEnd);
            }

            pipeline.DrawCurve(crv, clr, thickness);

        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outlist = new List<Guid>();
            var crv = this.GeneratePreviewGeometry(state, xform, out var clr, out var thickness);

            if (this.StartCurveEnd != null)

            {
                outlist.AddRange(this.StartCurveEnd.BakeRhinoPreview(tolerance, xform.Clone(), state, this.Curve.PointAtStart, this.Curve.TangentAtStart, doc, attr));
            }

            if (this.EndCurveEnd != null)
            {
                outlist.AddRange(this.EndCurveEnd.BakeRhinoPreview(tolerance, xform.Clone(), state, this.Curve.PointAtEnd, this.Curve.TangentAtEnd, doc, attr));
            }

            attr.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
            attr.ObjectColor = clr;
            attr.PlotWeightSource = Rhino.DocObjects.ObjectPlotWeightSource.PlotWeightFromObject;
            attr.PlotWeight = thickness;

            outlist.Add(doc.Objects.AddCurve(crv, attr));

            return outlist;

        }

        private Curve GeneratePreviewGeometry(DrawState state, Transform xform, out Color clr, out int thickness)
        {
            clr = this.Attributes.Colour;

            switch (state)
            {
                case DrawState.Normal:
                    break;
                case DrawState.Selected:
                    clr = Diagram.SelectedColor;

                    break;
                case DrawState.NoFills:
                    clr = Color.Transparent;
                    break;

            }

            thickness = (int)this.Attributes.LineWeight;
            if (thickness <= 0)
            {
                thickness = 1;
            }

            var drawCurve = this.Curve;
            if (xform != Transform.ZeroTransformation)
            {
                drawCurve = this.Curve.DuplicateCurve();
                drawCurve.Transform(xform);

            }

            return drawCurve;

        }

        public PointF[] GetPoints()
        {
            var polyc = this.Curve.ToPolyline(0.01, 0.01, 1, 1000);

            if (polyc == null)
            {
                return null;
            }

            var pts = new PointF[polyc.PointCount];
            for (var i = 0; i < polyc.PointCount; i++)
            {
                pts[i] = Diagram.ConvertPoint(polyc.Point(i));
            }

            return pts;

        }
    }
}
