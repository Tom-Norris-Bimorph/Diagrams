using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using DiagramLibrary.Text;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{

    public class DiagramDimension : IDiagramDimension
    {
        public IDimensionAttributes DimensionAttributes { get; }

        public IDiagramCurve Curve { get; }

        public IDiagramText Text { get; }

        public IDiagramCurveAttributes CurveAttributes => this.Curve.CurveAttributes;

        public IDiagramTextAttributes TextAttributes => this.Text.TextAttributes;

        public DiagramDimension(Curve curve, IDimensionAttributes dimensionAttributes,
            IDiagramCurveEnd curveEnds, IDiagramCurveAttributes curveAttributes, IDiagramTextAttributes textAttributes)
        {

            var offsetCurve = curve.Offset(Plane.WorldXY, dimensionAttributes.Offset,
            DiagramDefaults.Tolerance, CurveOffsetCornerStyle.None);

            var curveApproximation = new LineCurve(curve.PointAtStart, curve.PointAtEnd);

            var guaranteedCurve = offsetCurve is null || offsetCurve.Length == 0 ? curveApproximation : offsetCurve[0];

            var startCurveEnd = curveEnds.DuplicateCurveEnd();
            startCurveEnd.Flip();

            var diagramCurve = new DiagramCurve(guaranteedCurve, curveAttributes, startCurveEnd, curveEnds);

            var textString = dimensionAttributes.OverrideText == string.Empty
                ? Math.Round(curve.GetLength(), dimensionAttributes.RoundTo).ToString() + dimensionAttributes.Suffix
                : dimensionAttributes.OverrideText;

            var point = guaranteedCurve.PointAtNormalizedLength(0.5);

            var locationPoint = DiagramCoordinateSystem.ConvertPoint(point);

            var diagramLocation = new DiagramLocation(locationPoint);

            this.Curve = diagramCurve;
            this.DimensionAttributes = dimensionAttributes;
            this.Text = new DiagramText(textString, diagramLocation, textAttributes);

        }

        private DiagramDimension(IDiagramCurve curve, IDimensionAttributes dimensionAttributes, IDiagramText text)
        {
            this.Curve = curve;
            this.DimensionAttributes = dimensionAttributes;
            this.Text = text;
        }

        public IDiagramObject Duplicate()
        {
            return new DiagramDimension(this.Curve, this.DimensionAttributes, this.Text);
        }

        public void DrawBitmap(Graphics g)
        {
            this.Curve.DrawBitmap(g);

            this.Text.DrawBitmap(g);

        }

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, DrawState state)
        {
            this.Curve.DrawRhinoPreview(pipeline, tolerance, transform, state);

            this.Text.DrawRhinoPreview(pipeline, tolerance, transform, state);
            return;
        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();

            outList.AddRange(this.Curve.BakeRhinoPreview(tolerance, transform, state, doc, attr));

            outList.AddRange(this.Text.BakeRhinoPreview(tolerance, transform, state, doc, attr));
            return outList;
        }
    }
}
