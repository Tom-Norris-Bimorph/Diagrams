using DiagramLibrary.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramCurveEnd : IDrawableDiagramObject
    {

        IBaseCurveDiagramObject m_Object = null;

        private Point3d m_BasePoint = Point3d.Unset;
        private Vector3d m_BaseDirection = Vector3d.Unset;
        private bool m_Flipped = false;

        public DiagramCurveEnd DuplicateCurveEnd()
        {

            return this.Duplicate() as DiagramCurveEnd;
        }

        public IDiagramObject Duplicate()
        {
            return new DiagramCurveEnd(m_Object.Duplicate() as BaseCurveDiagramObject, m_BasePoint, m_BaseDirection, m_Flipped);
        }

        public DiagramCurveEnd(IBaseCurveDiagramObject diagramObject, Point3d location, Vector3d rotation, bool flipped)

        {
            m_Object = diagramObject.Duplicate() as BaseCurveDiagramObject;

            m_BasePoint = location;
            m_BaseDirection = rotation;
            m_Flipped = flipped;

        }

        public void Flip()
        {
            m_Flipped = !m_Flipped;
        }

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, DrawState state, Point3d location, Vector3d rotation)
        {

            var flipCorrectedDirection = m_BaseDirection;
            if (m_Flipped)
            {
                flipCorrectedDirection.Reverse();
            }

            var positionedObject = m_Object.SetLocationAndDirectionForDrawing(m_BasePoint, flipCorrectedDirection, location, rotation);
            if (positionedObject != null)
            {
                positionedObject.DrawRhinoPreview(pipeline, tolerance, transform, state);
            }
        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Point3d location, Vector3d rotation, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();

            var flipCorrectedDirection = m_BaseDirection;
            if (m_Flipped)
            {
                flipCorrectedDirection.Reverse();
            }

            var positionedObject = m_Object.SetLocationAndDirectionForDrawing(m_BasePoint, flipCorrectedDirection, location, rotation);
            if (positionedObject != null)
            {
                outList.AddRange(positionedObject.BakeRhinoPreview(tolerance, transform, state, doc, attr));
            }
            return outList;
        }

        public void DrawBitmap(Graphics g, Point3d location, Vector3d rotation)
        {
            var positionedObject = m_Object.SetLocationAndDirectionForDrawing(m_BasePoint, m_BaseDirection, location, rotation);
            if (positionedObject != null)
            {
                positionedObject.DrawBitmap(g);
            }
        }

        public static DiagramCurveEnd DefaultDimentionCurveEnd(double scale, Color color, float lineWieght)
        {

            var crvs = new List<Curve>();
            crvs.Add(new Line(Point3d.Origin, Vector3d.YAxis, 10 * scale).ToNurbsCurve());
            crvs.Add(new Line(Point3d.Origin, Vector3d.XAxis, 5 * scale).ToNurbsCurve());
            crvs.Add(new Line(Point3d.Origin, Vector3d.XAxis, -5 * scale).ToNurbsCurve());
            crvs.Add(new Line(Point3d.Origin, new Vector3d(1, 1, 0), 5 * scale).ToNurbsCurve());
            crvs.Add(new Line(Point3d.Origin, new Vector3d(1, 1, 0), -5 * scale).ToNurbsCurve());

            var crvCollection = DiagramCurveCollection.Create(crvs, color, lineWieght);
            var curveEnd = new DiagramCurveEnd(crvCollection, Point3d.Origin, Vector3d.YAxis, false);
            return curveEnd;

        }
    }
}
