﻿using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramDimention : DiagramCurve
    {
        private string m_suffix;
        private string m_OverrideText;
        private Color m_MaskColor;
        private float m_TextSize;
        private string m_FontName;
        private float m_Padding;
        private int m_RoundTo;
        private float m_offset;

        public new static DiagramCurve Create(Curve crv, Color colour, float lineWeight, float offset)
        {

            return Create(crv.PointAtStart, crv.PointAtEnd, Diagram.DefaultColor, Diagram.DefaultLineWeight, string.Empty, string.Empty, Color.Transparent, Diagram.DefaultTextScale, Diagram.DefaultFontName, Diagram.DefaultPadding, 2, DiagramCurveEnd.DefaultDimentionCurveEnd(1, Diagram.DefaultColor, Diagram.DefaultLineWeight), offset);
        }

        public static DiagramDimention Create(Point3d pt1, Point3d pt2, Color color, float lineWieght, float textSize, float offset)
        {

            return Create(pt1, pt2, color, lineWieght, string.Empty, string.Empty, Color.Transparent, textSize, Diagram.DefaultFontName, Diagram.DefaultPadding, 2, DiagramCurveEnd.DefaultDimentionCurveEnd(1, color, lineWieght), offset);
        }

        public static DiagramDimention Create(Point3d pt1, Point3d pt2, Color color, float lineWieght, string suffix, string overrideText, Color maskcolor, float textSize, string fontname, float padding, int roundTo, DiagramCurveEnd curveEnds, float offset)
        {

            var ln = new Line(pt1, pt2);
            var plane = Plane.WorldXY;
            plane.Origin = pt1;
            var angle = Vector3d.VectorAngle(plane.XAxis, ln.Direction, plane);
            plane.Rotate(angle, Vector3d.ZAxis);
            var rec = new Rectangle3d(plane, ln.Length, offset);
            var ln2 = new Line(rec.Corner(3), rec.Corner(2));
            if (offset < 0)
            {
                rec = new Rectangle3d(plane, new Interval(0, ln.Length), new Interval(offset, 0));
                ln2 = new Line(rec.Corner(0), rec.Corner(1));
            }

            var diagramDimention = new DiagramDimention();
            diagramDimention._colour = color;
            diagramDimention._lineWeight = lineWieght;
            diagramDimention.m_Curve = ln2.ToNurbsCurve();
            diagramDimention.m_suffix = suffix;
            diagramDimention.m_OverrideText = overrideText;
            diagramDimention.m_MaskColor = maskcolor;
            diagramDimention.m_TextSize = textSize;
            diagramDimention.m_FontName = fontname;
            diagramDimention.m_Padding = padding;
            diagramDimention.m_RoundTo = roundTo;
            diagramDimention.m_offset = offset;
            diagramDimention.AddCurveEnds(curveEnds.DuplicateCurveEnd(), curveEnds.DuplicateCurveEnd());
            diagramDimention.StartCurveEnd.Flip();

            return diagramDimention;
        }

        public override DiagramObject Duplicate()
        {
            var diagramDimention = new DiagramDimention();
            diagramDimention._colour = _colour;
            diagramDimention._lineWeight = _lineWeight;
            diagramDimention.m_Curve = m_Curve.DuplicateCurve();
            diagramDimention.m_suffix = m_suffix;
            diagramDimention.m_OverrideText = m_OverrideText;
            diagramDimention.m_MaskColor = m_MaskColor;
            diagramDimention.m_TextSize = m_TextSize;
            diagramDimention.m_FontName = m_FontName;
            diagramDimention.m_Padding = m_Padding;
            diagramDimention.m_RoundTo = m_RoundTo;
            diagramDimention.m_offset = m_offset;
            diagramDimention.StartCurveEnd = this.StartCurveEnd.DuplicateCurveEnd();
            diagramDimention.EndCurveEnd = this.EndCurveEnd.DuplicateCurveEnd();
            return diagramDimention;
        }

        private DiagramText getText()
        {
            var textValue = m_OverrideText + m_suffix;

            if (m_OverrideText == string.Empty)
            {
                textValue = Math.Round(m_Curve.GetLength(), m_RoundTo).ToString() + m_suffix;
            }
            var pt = m_Curve.PointAt(0.5);
            return DiagramText.Create(textValue, Diagram.ConvertPoint(pt), _colour, m_TextSize, TextJustification.MiddleCenter, m_MaskColor, _colour, _lineWeight, m_FontName, new SizeF(-1, -1), m_Padding, TextJustification.MiddleCenter);
        }

        public override void DrawBitmap(Graphics g)
        {
            base.DrawBitmap(g);

            this.getText().DrawBitmap(g);

        }

        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, DrawState state)
        {
            base.DrawRhinoPreview(pipeline, tolerance, transform, state);

            this.getText().DrawRhinoPreview(pipeline, tolerance, transform, state);
            return;
        }

        public override List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();

            outList.AddRange(base.BakeRhinoPreview(tolerance, transform, state, doc, attr));

            outList.AddRange(this.getText().BakeRhinoPreview(tolerance, transform, state, doc, attr));
            return outList;
        }
    }
}
