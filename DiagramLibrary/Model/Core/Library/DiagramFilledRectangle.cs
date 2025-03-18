using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramFilledRectangle : DiagramFilledCurve
    {


        public static DiagramFilledRectangle Create(Rectangle3d outerRectangle, Color colour, Color lineColour, float lineWeight)
        {
            var rectangle = new DiagramFilledRectangle();
            rectangle._colour = colour;
            rectangle._lineWeight = lineWeight;
            rectangle.m_LineColor = lineColour;


            rectangle.m_OuterCurves.Add(DiagramCurve.Create(outerRectangle.ToNurbsCurve(), lineColour, lineWeight));


            return rectangle;
        }

        public void UpdateRectangle(Rectangle3d newRectangle)
        {
            if (m_OuterCurves.Count > 0)
            {
                // Update the first (and only) outer curve with the new rectangle
                m_OuterCurves[0] = DiagramCurve.Create(newRectangle.ToNurbsCurve(), m_LineColor, _lineWeight);
            }
            else
            {
                // If there are no outer curves, add the new rectangle as the first outer curve
                m_OuterCurves.Add(DiagramCurve.Create(newRectangle.ToNurbsCurve(), m_LineColor, _lineWeight));
            }
        }

        public void UpdateRectangle(PointF origin, SizeF size)
        {
            var newRectangle = new Rectangle3d(new Plane(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis), Diagram.ConvertPoint(origin), new Point3d(origin.X + size.Width, origin.Y + size.Height, 0));
            this.UpdateRectangle(newRectangle);
        }

        public new DiagramFilledRectangle Duplicate()
        {
            var duplicatedRectangle = new DiagramFilledRectangle();

            duplicatedRectangle.m_OuterCurves = m_OuterCurves;
            duplicatedRectangle.m_InnerCurves = m_InnerCurves;


            duplicatedRectangle._colour = _colour;
            duplicatedRectangle._lineWeight = _lineWeight;

            duplicatedRectangle.m_LineColor = m_LineColor;

            return duplicatedRectangle;
        }
    }
}
