using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace DiagramLibrary
{
    public class DiagramCurve : BaseCurveDiagramObject
    {
        protected Curve m_Curve;
        protected DiagramCurveEnd m_StartCurveEnd = null;
        protected DiagramCurveEnd m_EndCurveEnd = null;



        public Curve Curve
        {
            get { return m_Curve; }
            set { m_Curve = value; }
        }

        public static DiagramCurve Create(Curve crv, Color Colour, float LineWeight)
        {
            DiagramCurve diagramCurve = new DiagramCurve();
            diagramCurve.m_Colour = Colour;
            diagramCurve.m_LineWeight = LineWeight;
            diagramCurve.m_Curve = crv;


            return diagramCurve;
        }

        public DiagramCurve DuplicateDiagramCurve()
        {
            return Duplicate() as DiagramCurve;
        }


        public override DiagramObject Duplicate()
        {
            DiagramCurve diagramCurve = new DiagramCurve();
            diagramCurve.m_Colour = m_Colour;
            diagramCurve.m_LineWeight = m_LineWeight;
            diagramCurve.m_Curve = m_Curve.DuplicateCurve();
            if (m_StartCurveEnd != null)
            {
                diagramCurve.m_StartCurveEnd = m_StartCurveEnd.DuplicateCurveEnd();

            }

            if (m_EndCurveEnd != null)
            {
                diagramCurve.m_EndCurveEnd = m_EndCurveEnd.DuplicateCurveEnd();
            }


            return diagramCurve;
        }

        public override BoundingBox GetBoundingBox()
        {
            return this.m_Curve.GetBoundingBox(true);
        }

        public void AddCurveEnds(BaseCurveDiagramObject start, Point3d setPointStart, Vector3d setDirectionStart, BaseCurveDiagramObject end, Point3d setPointEnd, Vector3d setDirectionEnd)

        {

            if (start != null)
            {
                m_StartCurveEnd = new DiagramCurveEnd(start, setPointStart, setDirectionStart, true);

            }

            if (end != null)
            {
                m_EndCurveEnd = new DiagramCurveEnd(end, setPointEnd, setDirectionEnd, false);

            }


        }

        public void AddCurveEnds(DiagramCurveEnd start, DiagramCurveEnd end)

        {

            if (start != null)
            {
                m_StartCurveEnd = start;

            }

            if (end != null)
            {
                m_EndCurveEnd = end;

            }


        }




        public override BaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation)
        {
            if (baseDirection == Vector3d.Unset)
            {
                return null;
            }



            DiagramCurve clone = Duplicate() as DiagramCurve;

            clone.m_Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
            double angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);

            clone.m_Curve.Rotate(angle, Plane.WorldXY.Normal, location);

            return clone;
        }



        public override void DrawBitmap( Graphics g)
        {

            if (m_StartCurveEnd != null)

            {
                m_StartCurveEnd.DrawBitmap(g, m_Curve.PointAtStart, m_Curve.TangentAtStart);
            }



            if (m_EndCurveEnd != null)
            {
                m_EndCurveEnd.DrawBitmap(g, m_Curve.PointAtEnd, m_Curve.TangentAtEnd);

            }





            PointF[] pts = GetPoints();
            if (pts != null)
            {
                g.DrawLines(this.GetPen(), pts);
            }



        }

        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
            
            Curve crv = GeneratePreviewGeometry(state, xform, out Color clr, out int thickness);

            if (m_StartCurveEnd != null)

            {
                m_StartCurveEnd.DrawRhinoPreview(pipeline, tolerance, xform.Clone(), state, m_Curve.PointAtStart, m_Curve.TangentAtStart);
            }


            if (m_EndCurveEnd != null)
            {
                m_EndCurveEnd.DrawRhinoPreview( pipeline, tolerance, xform.Clone(), state, m_Curve.PointAtEnd, m_Curve.TangentAtEnd);
            }
            
                       
           pipeline.DrawCurve(crv, clr, thickness);
           
            }


        public override List<Guid> BakeRhinoPreview( double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            List<Guid> outlist = new List<Guid>();
            Curve crv = GeneratePreviewGeometry(state, xform, out Color clr, out int thickness);



            if (m_StartCurveEnd != null)

            {
                outlist.AddRange(m_StartCurveEnd.BakeRhinoPreview(tolerance, xform.Clone(), state, m_Curve.PointAtStart, m_Curve.TangentAtStart, doc, attr));
            }


            if (m_EndCurveEnd != null)
            {
                outlist.AddRange(m_EndCurveEnd.BakeRhinoPreview(tolerance, xform.Clone(), state, m_Curve.PointAtEnd, m_Curve.TangentAtEnd, doc, attr));
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
           
            clr = m_Colour;

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


           
             thickness = (int)this.m_LineWeight;
            if (thickness <= 0)
            {
                thickness = 1;
            }

            Curve drawCurve = m_Curve;
            if (xform != Transform.ZeroTransformation)
            {
                drawCurve = m_Curve.DuplicateCurve();
                drawCurve.Transform(xform);


            }

            
            return drawCurve;

        }


        public PointF[] GetPoints()
        {
            PolylineCurve polyc = m_Curve.ToPolyline(0.01, 0.01, 1, 1000);

            if (polyc == null)
            {
                return null;
            }

            PointF[] pts = new PointF[polyc.PointCount];
            for (int i = 0; i < polyc.PointCount; i++)
            {
                pts[i] = Diagram.ConvertPoint(polyc.Point(i));
            }

            return pts;


        }

        public Curve GetCurve()
        {
            return m_Curve;
        }



    }
}
