using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
   public  class DiagramCurveCollection : BaseCurveDiagramObject
    {
      
        protected List<DiagramCurve> m_Curves = new List<DiagramCurve>();





        public static DiagramCurveCollection Create(List<Curve> curves, Color colour, float lineWeight)
        {

            DiagramCurveCollection diagramCurveCollection = new DiagramCurveCollection();
            diagramCurveCollection.m_Colour = colour;
            diagramCurveCollection.m_LineWeight = lineWeight;

            for (int i = 0; i < curves.Count; i++)
            {
                diagramCurveCollection.m_Curves.Add(DiagramCurve.Create(curves[i], colour, lineWeight));
            }
                       
            return diagramCurveCollection;
        }




        public override DiagramObject Duplicate()
        {
            DiagramCurveCollection diagramCurveCollection = new DiagramCurveCollection();
            diagramCurveCollection.m_Colour = m_Colour;
            diagramCurveCollection.m_LineWeight = m_LineWeight;
                

            for (int i = 0; i < m_Curves.Count; i++)
            {
                diagramCurveCollection.m_Curves.Add(m_Curves[i].DuplicateDiagramCurve());
            }
                     
          
            return diagramCurveCollection;
        }



        public override BaseCurveDiagramObject SetLocationAndDirectionForDrawing(Point3d basePoint, Vector3d baseDirection, Point3d location, Vector3d rotation)
        {

            if (baseDirection == Vector3d.Unset)
            {
                return null;
            }


            DiagramCurveCollection clone = Duplicate() as DiagramCurveCollection;
         

            for (int i = 0; i < clone.m_Curves.Count; i++)

            {
                clone.m_Curves[i].Curve.Translate(new Vector3d(location.X - basePoint.X, location.Y - basePoint.Y, 0));
                double angle = Vector3d.VectorAngle(baseDirection, rotation, Plane.WorldXY);
               clone.m_Curves[i].Curve.Rotate(angle, Plane.WorldXY.Normal, location);
                            }

            return clone;
        }


        public override BoundingBox GetBoundingBox()
        {
            BoundingBox bbox = BoundingBox.Empty;
          
            for (int i = 0; i < this.m_Curves.Count; i++)
            {
                bbox.Union(this.m_Curves[i].GetBoundingBox());
            }

            return bbox;
        }


       


        public override PointF GetBoundingBoxLocation()
        {
            BoundingBox bbox = GetBoundingBox();
            return Diagram.ConvertPoint(bbox.Min);
        }



        public override void DrawBitmap(  Graphics g)

        {
            
            foreach (DiagramCurve crv in m_Curves)
            {

                crv.DrawBitmap(g);
            }
            
        }


        public override void DrawRhinoPreview( Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
          
            foreach (DiagramCurve crv in m_Curves)
            {

               crv.DrawRhinoPreview(pipeline,  tolerance,  xform,  state);
            }
            return ;


        }

        public override List<Guid> BakeRhinoPreview( double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            List<Guid> outList = new List<Guid>();
            foreach (DiagramCurve crv in m_Curves)
            {

                outList.AddRange(crv.BakeRhinoPreview( tolerance, xform, state, doc, attr));
            }
            return outList;


        }




    }
}
