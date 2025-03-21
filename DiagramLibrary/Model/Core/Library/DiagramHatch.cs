using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DiagramLibrary
{

    public class DiagramHatch : IDiagramHatch
    {
        public Rhino.Display.DisplayMaterial CachedMaterial { get; }
        public Rhino.Display.DisplayMaterial CachedSelectedMaterial { get; }

        public IList<IDiagramCurve> InnerCurves { get; }
        public IList<IDiagramCurve> OuterCurves { get; }

        public IDiagramHatchAttributes DiagramHatchAttributes { get; }

        public IDiagramCurveAttributes DiagramCurveAttributes { get; }

        public DiagramHatch(IList<IDiagramCurve> outerCurves, IList<IDiagramCurve> innerCurves,
            IDiagramCurveAttributes diagramCurveAttributes, IDiagramHatchAttributes hatchAttributes)
        {
            this.DiagramHatchAttributes = hatchAttributes;
            this.DiagramCurveAttributes = diagramCurveAttributes;
            this.InnerCurves = new List<IDiagramCurve>();
            this.OuterCurves = new List<IDiagramCurve>();

            foreach (var curve in outerCurves)
            {
                this.OuterCurves.Add(curve);
            }

            foreach (var curve in innerCurves)
            {
                this.InnerCurves.Add(curve);
            }
        }

        public DiagramHatch(Curve[] outerCurves, Curve[] innerCurves, IDiagramCurveAttributes diagramCurveAttributes,
            IDiagramHatchAttributes hatchAttributes)
        {
            this.DiagramHatchAttributes = hatchAttributes;
            this.DiagramCurveAttributes = diagramCurveAttributes;
            this.InnerCurves = new List<IDiagramCurve>();
            this.OuterCurves = new List<IDiagramCurve>();

            foreach (var curve in outerCurves)
            {
                this.OuterCurves.Add(new DiagramCurve(curve, diagramCurveAttributes));
            }

            foreach (var curve in innerCurves)
            {
                this.InnerCurves.Add(new DiagramCurve(curve, diagramCurveAttributes));
            }
        }

        public DiagramHatch(Curve outerCurve, IDiagramCurveAttributes diagramCurveAttributes, IDiagramHatchAttributes hatchAttributes)
            : this(new Curve[] { outerCurve }, new Curve[] { }, diagramCurveAttributes, hatchAttributes)
        { }

        public static List<DiagramHatch> CreateFromBrep(Brep brep, IDiagramCurveAttributes diagramCurveAttributes, IDiagramHatchAttributes hatchAttributes)
        {
            var hatches = new List<DiagramHatch>();

            foreach (var face in brep.Faces)
            {
                var innerCurves = face.DuplicateFace(false).DuplicateNakedEdgeCurves(false, true);
                var outerCurves = face.DuplicateFace(false).DuplicateNakedEdgeCurves(true, false);

                var filledCurve = new DiagramHatch(outerCurves, innerCurves, diagramCurveAttributes, hatchAttributes);
                hatches.Add(filledCurve);
            }

            return hatches;
        }

        public IDiagramObject Duplicate()
        {
            var innerCurvesCopy = new List<IDiagramCurve>();
            var outerCurvesCopy = new List<IDiagramCurve>();

            foreach (var curve in this.OuterCurves)
            {
                outerCurvesCopy.Add(curve.DuplicateDiagramCurve());
            }

            foreach (var curve in this.InnerCurves)
            {
                innerCurvesCopy.Add(curve.DuplicateDiagramCurve());
            }

            var diagramFilledCurve = new DiagramHatch(outerCurvesCopy, innerCurvesCopy, this.DiagramCurveAttributes, this.DiagramHatchAttributes);

            return diagramFilledCurve;
        }

        public IBaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation)
        {
            if (baseDirection == Vector3d.Unset) return null;

            var clone = this.Duplicate() as DiagramHatch;

            foreach (var innerCurve in clone.InnerCurves)
            {
                innerCurve.Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
                var angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);
                innerCurve.Curve.Rotate(angle, Plane.WorldXY.Normal, location);
            }

            foreach (var outCurve in clone.OuterCurves)
            {
                outCurve.Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
                var angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);
                outCurve.Curve.Rotate(angle, Plane.WorldXY.Normal, location);
            }
            return clone;
        }

        public BoundingBox GetBoundingBox()
        {
            var boundingBox = BoundingBox.Empty;
            foreach (var innerCurve in this.InnerCurves)
            {
                boundingBox.Union(innerCurve.GetBoundingBox());
            }

            foreach (var outerCurve in this.OuterCurves)
            {
                boundingBox.Union(outerCurve.GetBoundingBox());
            }

            return boundingBox;
        }

        public PointF GetBoundingBoxLocation()
        {
            var boundingBox = this.GetBoundingBox();
            return DiagramCoordinateSystem.ConvertPoint(boundingBox.Min);
        }

        public void DrawBitmap(Graphics g)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();

            var points3d = new List<Point3d>();

            foreach (var curve in this.OuterCurves)
            {

                var pts = curve.GetPoints();
                if (pts == null) { continue; }
                path.AddLines(pts);
            }

            foreach (var curve in this.InnerCurves)

            {
                var holdPath = new System.Drawing.Drawing2D.GraphicsPath();
                var pts = curve.GetPoints();

                holdPath.AddLines(pts);

                path.AddPath(holdPath, false);
            }

            g.FillPath(this.DiagramHatchAttributes.GetBrush(), path);

            if (this.DiagramCurveAttributes.LineWeight > 0)
            {
                g.DrawPath(this.DiagramCurveAttributes.GetPen(), path);

            }
        }

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
            var breps = this.GeneratePreviewGeometry(tolerance, xform, state, out var clr, out var material, out var drawLines);

            foreach (var item in breps)
            {
                pipeline.DrawBrepShaded(item, material);
            }

            if (drawLines)
            {
                foreach (var item in this.OuterCurves)
                {
                    item.DrawRhinoPreview(pipeline, tolerance, xform, state);
                }
                foreach (var item in this.InnerCurves)
                {
                    item.DrawRhinoPreview(pipeline, tolerance, xform, state);
                }
            }
            return;
        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();
            var breps = this.GeneratePreviewGeometry(tolerance, transform, state, out var clr, out var material, out var drawLines);

            if (state != DrawState.NoFills)
            {
                if (clr != Color.Transparent)
                {

                    foreach (var item in breps)

                    {

                        attr.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
                        attr.ObjectColor = clr;
                        var name = "DiagramsMaterial_" + material.Diffuse.R.ToString() + "_" + material.Diffuse.G.ToString() + "_" + material.Diffuse.B.ToString() + "_" + material.Transparency.ToString();

                        var materialIndex = doc.Materials.Find(name, true);

                        if (materialIndex < 0)
                        {
                            var rhinoMaterial = new Rhino.DocObjects.Material();
                            rhinoMaterial.DiffuseColor = material.Diffuse;
                            rhinoMaterial.Transparency = material.Transparency;

                            rhinoMaterial.Name = name;
                            materialIndex = doc.Materials.Add(rhinoMaterial, false);
                        }

                        attr.MaterialSource = Rhino.DocObjects.ObjectMaterialSource.MaterialFromObject;
                        attr.MaterialIndex = materialIndex;

                        outList.Add(doc.Objects.AddBrep(item, attr));

                    }
                }
            }

            if (drawLines)
            {
                foreach (var item in this.OuterCurves)
                {
                    outList.AddRange(item.BakeRhinoPreview(tolerance, transform, state, doc, attr));
                }
                foreach (var item in this.InnerCurves)
                {
                    outList.AddRange(item.BakeRhinoPreview(tolerance, transform, state, doc, attr));
                }
            }
            return outList;
        }

        public List<Brep> GeneratePreviewGeometry(double tolerance, Transform transform, DrawState state, out Color colour, out Rhino.Display.DisplayMaterial material, out bool drawLines)
        {
            var outlist = new List<Brep>();
            colour = this.DiagramCurveAttributes.Colour;
            drawLines = this.DiagramCurveAttributes.LineWeight > 0;
            material = null;

            switch (state)
            {
                case DrawState.Normal:
                    if (this.CachedMaterial == null)
                    {
                        this.CachedMaterial = new Rhino.Display.DisplayMaterial(colour, 1.0 - (colour.A / 255));
                    }

                    material = this.CachedMaterial;
                    break;
                case DrawState.Selected:
                    colour = DiagramDefaults.SelectedColor;
                    drawLines = true;
                    if (this.CachedSelectedMaterial == null)
                    {
                        this.CachedSelectedMaterial = new Rhino.Display.DisplayMaterial(colour, 1.0 - (colour.A / 255));
                    }
                    material = this.CachedSelectedMaterial;
                    break;
                case DrawState.NoFills:

                    break;

            }

            var diagramCurves = new List<IDiagramCurve>();
            diagramCurves.AddRange(this.OuterCurves);
            diagramCurves.AddRange(this.InnerCurves);

            var curves = diagramCurves.Select(diagramCurve => diagramCurve.Curve).ToArray();
            var breps = Brep.CreatePlanarBreps(curves, tolerance);

            if (breps == null) return outlist;

            foreach (var item in breps)
            {
                if (transform != Transform.ZeroTransformation)
                {
                    item.Transform(transform);
                }
                outlist.Add(item);
            }

            return outlist;
        }
    }
}
