namespace DiagramLibrary
{/*
    public class DiagramCurveCollection : IBaseCurveDiagramObject
    {

        private readonly List<DiagramCurve> _curves = new List<DiagramCurve>();

        public IDiagramCurveAttributes CurveAttributes { get; }

        public DiagramCurveCollection(List<Curve> curves, IDiagramCurveAttributes curveAttributes)
        {
            this.CurveAttributes = curveAttributes;

            foreach (var curve in curves)
            {
                var diagramCurve = new DiagramCurve(curve, curveAttributes);

                _curves.Add(diagramCurve);
            }
        }

        public SizeF GetTotalSize()
        {
            throw new NotImplementedException();
        }

        IBaseCurveDiagramObject IBaseCurveDiagramObject.SetLocationAndDirectionForDrawing(Point3d basePoint,
            Vector3d baseDirection, Point3d location, Vector3d rotation)
        {
            throw new NotImplementedException();
        }

        public IDiagramObject Duplicate()
        {
            var diagramCurveCollection = new DiagramCurveCollection();
            diagramCurveCollection._colour = _colour;
            diagramCurveCollection._lineWeight = _lineWeight;

            for (var i = 0; i < _curves.Count; i++)
            {
                diagramCurveCollection._curves.Add(_curves[i].DuplicateDiagramCurve());
            }

            return diagramCurveCollection;
        }

        public override BaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation)
        {

            if (baseDirection == Vector3d.Unset)
            {
                return null;
            }

            var clone = this.Duplicate() as DiagramCurveCollection;

            for (var i = 0; i < clone._curves.Count; i++)

            {
                clone._curves[i].Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
                var angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);
                clone._curves[i].Curve.Rotate(angle, Plane.WorldXY.Normal, location);
            }

            return clone;
        }

        public override BoundingBox GetBoundingBox()
        {
            var bbox = BoundingBox.Empty;

            for (var i = 0; i < _curves.Count; i++)
            {
                bbox.Union(_curves[i].GetBoundingBox());
            }

            return bbox;
        }

        public override PointF GetBoundingBoxLocation()
        {
            var bbox = this.GetBoundingBox();
            return Diagram.ConvertPoint(bbox.Min);
        }

        public override void DrawBitmap(Graphics g)

        {

            foreach (var crv in _curves)
            {

                crv.DrawBitmap(g);
            }
        }

        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {

            foreach (var crv in _curves)
            {

                crv.DrawRhinoPreview(pipeline, tolerance, xform, state);
            }
            return;

        }

        public override List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();
            foreach (var crv in _curves)
            {

                outList.AddRange(crv.BakeRhinoPreview(tolerance, transform, state, doc, attr));
            }
            return outList;

        }
    }
}*/
