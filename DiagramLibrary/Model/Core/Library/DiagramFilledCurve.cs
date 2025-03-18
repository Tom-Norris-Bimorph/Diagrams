using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DiagramLibrary
{
    public class DiagramFilledCurve : BaseCurveDiagramObject
    {

        protected Color m_LineColor;
        protected Color m_BackColour;
        protected System.Drawing.Drawing2D.HatchStyle m_hatchStyle;
        protected bool isSolid = true;

        protected double m_hatchRotation = 0;

        protected double m_hatchScale = 1;

        protected Rhino.Display.DisplayMaterial m_CachedMaterial = null;
        protected Rhino.Display.DisplayMaterial m_CachedSelectedMaterial = null;

        protected List<DiagramCurve> m_InnerCurves = new List<DiagramCurve>();
        protected List<DiagramCurve> m_OuterCurves = new List<DiagramCurve>();

        public Color BackColour
        {
            get { return m_BackColour; }
            set
            {
                m_BackColour = value;
                m_CachedMaterial = null;
            }
        }

        public override Color Colour
        {
            get { return _colour; }
            set
            {
                _colour = value;
                m_CachedMaterial = null;
            }
        }

        public static DiagramFilledCurve Create(Curve curve, Color Colour, Color LineColour, float LineWeight)
        {

            var diagramFilledCurve = new DiagramFilledCurve();
            diagramFilledCurve._colour = Colour;
            diagramFilledCurve._lineWeight = LineWeight;

            diagramFilledCurve.m_LineColor = LineColour;

            diagramFilledCurve.m_OuterCurves.Add(DiagramCurve.Create(curve, LineColour, LineWeight));

            return diagramFilledCurve;
        }

        public static DiagramFilledCurve Create(Curve[] OuterCurves, Curve[] InnerCurves, Color Colour, Color LineColour, float LineWeight)
        {

            var diagramFilledCurve = new DiagramFilledCurve();
            diagramFilledCurve._colour = Colour;
            diagramFilledCurve._lineWeight = LineWeight;

            diagramFilledCurve.m_LineColor = LineColour;

            for (var i = 0; i < OuterCurves.Length; i++)
            {
                diagramFilledCurve.m_OuterCurves.Add(DiagramCurve.Create(OuterCurves[i], LineColour, LineWeight));
            }
            if (InnerCurves != null)
            {
                for (var i = 0; i < InnerCurves.Length; i++)
                {
                    diagramFilledCurve.m_InnerCurves.Add(DiagramCurve.Create(InnerCurves[i], LineColour, LineWeight));
                }
            }

            return diagramFilledCurve;
        }

        public static List<DiagramFilledCurve> CreateFromBrep(Brep brep, Color Colour, Color LineColour, float LineWeight)
        {
            var hatches = new List<DiagramFilledCurve>();

            for (var i = 0; i < brep.Faces.Count; i++)
            {

                var crvsInner = brep.Faces[i].DuplicateFace(false).DuplicateNakedEdgeCurves(false, true);
                var crvsOuter = brep.Faces[i].DuplicateFace(false).DuplicateNakedEdgeCurves(true, false);

                var dHatch = DiagramFilledCurve.Create(crvsOuter, crvsInner, Colour, LineColour, LineWeight);
                hatches.Add(dHatch);

            }

            return hatches;
        }

        public override DiagramObject Duplicate()
        {
            var diagramFilledCurve = new DiagramFilledCurve();
            diagramFilledCurve._colour = _colour;
            diagramFilledCurve._lineWeight = _lineWeight;
            diagramFilledCurve.m_LineColor = m_LineColor;

            for (var i = 0; i < m_OuterCurves.Count; i++)
            {
                diagramFilledCurve.m_OuterCurves.Add(m_OuterCurves[i].DuplicateDiagramCurve());
            }

            for (var i = 0; i < m_InnerCurves.Count; i++)
            {
                diagramFilledCurve.m_InnerCurves.Add(m_InnerCurves[i].DuplicateDiagramCurve());
            }

            return diagramFilledCurve;
        }

        public override BaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation)
        {

            if (baseDirection == Vector3d.Unset)
            {
                return null;
            }

            var clone = this.Duplicate() as DiagramFilledCurve;

            for (var i = 0; i < clone.m_InnerCurves.Count; i++)

            {

                clone.m_InnerCurves[i].Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
                var angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);
                clone.m_InnerCurves[i].Curve.Rotate(angle, Plane.WorldXY.Normal, location);

            }

            for (var i = 0; i < clone.m_OuterCurves.Count; i++)

            {

                clone.m_OuterCurves[i].Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
                var angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);
                clone.m_OuterCurves[i].Curve.Rotate(angle, Plane.WorldXY.Normal, location);

            }

            return clone;
        }

        public Brush GetBrush()
        {
            if (isSolid)
            {
                return new SolidBrush(_colour);

            }
            else
            {
                return new System.Drawing.Drawing2D.HatchBrush(m_hatchStyle, _colour, m_BackColour);
            }
        }

        public override BoundingBox GetBoundingBox()
        {
            var bbox = BoundingBox.Empty;
            for (var i = 0; i < m_InnerCurves.Count; i++)
            {
                bbox.Union(m_InnerCurves[i].GetBoundingBox());
            }

            for (var i = 0; i < m_OuterCurves.Count; i++)
            {
                bbox.Union(m_OuterCurves[i].GetBoundingBox());
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

            var path = new System.Drawing.Drawing2D.GraphicsPath();

            var points3d = new List<Point3d>();

            foreach (var crv in m_OuterCurves)
            {

                var pts = crv.GetPoints();
                if (pts == null) { continue; }
                path.AddLines(pts);
            }

            foreach (var crv in m_InnerCurves)

            {
                var holdPath = new System.Drawing.Drawing2D.GraphicsPath();
                var pts = crv.GetPoints();

                holdPath.AddLines(pts);

                path.AddPath(holdPath, false);
            }

            g.FillPath(this.GetBrush(), path);

            if (_lineWeight > 0)
            {
                g.DrawPath(this.GetPen(), path);

            }
        }

        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
            var breps = this.GeneratePreviewGeometry(tolerance, xform, state, out var clr, out var material, out var drawLines);

            foreach (var item in breps)

            {

                pipeline.DrawBrepShaded(item, material);

            }

            if (drawLines)
            {
                foreach (var item in m_OuterCurves)
                {
                    item.DrawRhinoPreview(pipeline, tolerance, xform, state);
                }
                foreach (var item in m_InnerCurves)
                {
                    item.DrawRhinoPreview(pipeline, tolerance, xform, state);
                }
            }
            return;
        }

        public override List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();
            var breps = this.GeneratePreviewGeometry(tolerance, xform, state, out var clr, out var material, out var drawLines);

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
                foreach (var item in m_OuterCurves)
                {
                    outList.AddRange(item.BakeRhinoPreview(tolerance, xform, state, doc, attr));
                }
                foreach (var item in m_InnerCurves)
                {
                    outList.AddRange(item.BakeRhinoPreview(tolerance, xform, state, doc, attr));
                }
            }
            return outList;
        }

        public List<Brep> GeneratePreviewGeometry(double tolerance, Transform xform, DrawState state, out Color clr, out Rhino.Display.DisplayMaterial material, out bool drawLines)
        {
            var outlist = new List<Brep>();
            clr = _colour;
            drawLines = _lineWeight > 0;
            material = null;

            switch (state)
            {
                case DrawState.Normal:
                    if (m_CachedMaterial == null)
                    {
                        m_CachedMaterial = new Rhino.Display.DisplayMaterial(clr, 1.0 - (clr.A / 255));
                    }

                    material = m_CachedMaterial;
                    break;
                case DrawState.Selected:
                    clr = Diagram.SelectedColor;
                    drawLines = true;
                    if (m_CachedSelectedMaterial == null)
                    {
                        m_CachedSelectedMaterial = new Rhino.Display.DisplayMaterial(clr, 1.0 - (clr.A / 255));
                    }
                    material = m_CachedSelectedMaterial;
                    break;
                case DrawState.NoFills:

                    break;

            }

            var dcrvs = new List<DiagramCurve>();
            dcrvs.AddRange(m_OuterCurves);
            dcrvs.AddRange(m_InnerCurves);
            var crvs = dcrvs.Select(x => (Curve)x.GetCurve()).ToArray();
            var breps = Brep.CreatePlanarBreps(crvs, tolerance);
            if (breps != null)
            {

                foreach (var item in breps)
                {
                    if (xform != Transform.ZeroTransformation)
                    {
                        item.Transform(xform);

                    }

                    outlist.Add(item);

                }
            }


            return outlist;
        }

        public new Pen GetPen()
        {
            return new Pen(m_LineColor, _lineWeight);
        }
    }
}
