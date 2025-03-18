using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramPointLabel : DiagramObject
    {

        private PointF m_PointLocation;
        private DiagramText m_DiagramText;
        private DiagramFilledCurve m_Mask;

        public DiagramText DiagramText
        {
            get { return m_DiagramText; }
            set { m_DiagramText = value; }
        }

        public DiagramFilledCurve Mask
        {
            get { return m_Mask; }
            set { m_Mask = value; }
        }


        public static DiagramPointLabel Create(string text, PointF leaderLocation, Color colour, float textSize,
          Color maskColour, Color frameColor, float frameLineWeight,
          string fontName, float padding)
        {
            DiagramPointLabel diagramLabel = new DiagramPointLabel();
            diagramLabel.m_DiagramText = DiagramText.Create(text, leaderLocation, colour, textSize / Math.Max(text.Length / 1.667f, 1f), Rhino.Geometry.TextJustification.MiddleCenter, Color.Transparent, Color.Transparent, 0, fontName, new SizeF(textSize * 4, textSize * 4), padding, Rhino.Geometry.TextJustification.MiddleCenter);
            diagramLabel.m_PointLocation = leaderLocation;
            diagramLabel. m_Mask = DiagramFilledCurve.Create(new Circle(new Plane(new Point3d(leaderLocation.X, leaderLocation.Y, 0),Plane.WorldXY.ZAxis),textSize).ToNurbsCurve(), maskColour, frameColor, frameLineWeight);

            return diagramLabel;
        }

       

        public override DiagramObject Duplicate()
        {
            DiagramPointLabel diagramLabel = new DiagramPointLabel();
            diagramLabel.m_DiagramText = m_DiagramText.Duplicate() as DiagramText;
            diagramLabel.m_PointLocation = m_PointLocation;
            diagramLabel.m_Mask = m_Mask;
            return diagramLabel;
        }

        public override BoundingBox GetBoundingBox()
        {
            BoundingBox bb = new BoundingBox(new Point3d[] { Diagram.ConvertPoint(m_PointLocation) });
            bb.Union(m_DiagramText.GetBoundingBox());
            bb.Union(m_Mask.GetBoundingBox());
            return bb;
        }

        public override void DrawBitmap(Graphics g)
        {
            m_Mask.DrawBitmap( g);
            m_DiagramText.DrawBitmap( g);
        }

        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
             m_Mask.DrawRhinoPreview(pipeline,  tolerance,  xform,  state);
            m_DiagramText.DrawRhinoPreview(pipeline, tolerance, xform, state);
            return;
        }

        public override List<Guid> BakeRhinoPreview( double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            List<Guid> outList = new List<Guid>();
            outList.AddRange(m_Mask.BakeRhinoPreview( tolerance, xform, state, doc, attr));
            outList.AddRange(m_DiagramText.BakeRhinoPreview( tolerance, xform, state, doc, attr));
            return outList;
        }


    }
}
