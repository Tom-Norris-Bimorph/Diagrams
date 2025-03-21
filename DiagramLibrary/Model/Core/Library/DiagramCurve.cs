using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramCurve : IDiagramCurve
    {
        public Curve Curve { get; }

        public IDiagramCurveEnd StartCurveEnd { get; }

        public IDiagramCurveEnd EndCurveEnd { get; }

        public IDiagramCurveAttributes CurveAttributes { get; }

        public IDiagramLocation Location => new DiagramLocation(DiagramCoordinateSystem.ConvertPoint(this.Curve.PointAtStart));

        public DiagramCurve(Curve curve, IDiagramCurveAttributes diagramCurveAttributes, IDiagramCurveEnd startCurveEnd = null, IDiagramCurveEnd endCurveEnd = null)
        {
            this.Curve = curve.DuplicateCurve();
            this.CurveAttributes = diagramCurveAttributes;
            this.StartCurveEnd = startCurveEnd is null ? DiagramDefaults.DefaultCurveEnd : startCurveEnd.Duplicate();
            this.EndCurveEnd = endCurveEnd is null ? DiagramDefaults.DefaultCurveEnd : endCurveEnd.Duplicate();
        }

        public IDiagramCurve DuplicateDiagramCurve()
        {
            return this.Duplicate() as DiagramCurve;
        }

        public IDiagramObject Duplicate()
        {
            return new DiagramCurve(this.Curve, this.CurveAttributes, this.StartCurveEnd, this.EndCurveEnd);
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
            return this.Curve.GetBoundingBox(true);
        }

        /*  public void AddCurveEnds(IBaseCurveDiagramObject start, Point3d setPointStart, Vector3d setDirectionStart,
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
          }*/

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
            this.StartCurveEnd.DrawBitmap(g, this.Curve.PointAtStart, this.Curve.TangentAtStart);

            this.EndCurveEnd.DrawBitmap(g, this.Curve.PointAtEnd, this.Curve.TangentAtEnd);

            var pts = this.GetPoints();
            if (pts != null)
            {
                g.DrawLines(this.CurveAttributes.GetPen(), pts);
            }
        }

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
            var crv = this.GeneratePreviewGeometry(state, xform, out var clr, out var thickness);

            this.StartCurveEnd.DrawRhinoPreview(pipeline, tolerance, xform.Clone(), state, this.Curve.PointAtStart, this.Curve.TangentAtStart);

            this.EndCurveEnd.DrawRhinoPreview(pipeline, tolerance, xform.Clone(), state, this.Curve.PointAtEnd, this.Curve.TangentAtEnd);

            pipeline.DrawCurve(crv, clr, thickness);

        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outlist = new List<Guid>();
            var crv = this.GeneratePreviewGeometry(state, transform, out var clr, out var thickness);

            outlist.AddRange(this.StartCurveEnd.BakeRhinoPreview(tolerance, transform.Clone(), state, this.Curve.PointAtStart, this.Curve.TangentAtStart, doc, attr));

            outlist.AddRange(this.EndCurveEnd.BakeRhinoPreview(tolerance, transform.Clone(), state, this.Curve.PointAtEnd, this.Curve.TangentAtEnd, doc, attr));

            attr.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
            attr.ObjectColor = clr;
            attr.PlotWeightSource = Rhino.DocObjects.ObjectPlotWeightSource.PlotWeightFromObject;
            attr.PlotWeight = thickness;

            outlist.Add(doc.Objects.AddCurve(crv, attr));

            return outlist;

        }

        private Curve GeneratePreviewGeometry(DrawState state, Transform xform, out Color clr, out int thickness)
        {
            clr = this.CurveAttributes.Colour;

            switch (state)
            {
                case DrawState.Normal:
                    break;
                case DrawState.Selected:
                    clr = DiagramDefaults.SelectedColor;

                    break;
                case DrawState.NoFills:
                    clr = Color.Transparent;
                    break;

            }

            thickness = (int)this.CurveAttributes.LineWeight;
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
            var polylineCurve = this.Curve.ToPolyline(0.01, 0.01, 1, 1000);

            if (polylineCurve == null)
            {
                return null;
            }

            var pts = new PointF[polylineCurve.PointCount];
            for (var i = 0; i < polylineCurve.PointCount; i++)
            {
                pts[i] = DiagramCoordinateSystem.ConvertPoint(polylineCurve.Point(i));
            }

            return pts;

        }
    }
}
