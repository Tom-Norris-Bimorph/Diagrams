namespace DiagramLibrary
{
} /*
    public class DiagramPieChart : DiagramCachedClass
    {
        List<double> m_Data = new List<double>();
        List<string> m_DataLabels = new List<string>();
        List<Color> m_DataFillColors = new List<Color>();
        List<Color> m_DataLineColors = new List<Color>();

        float m_OuterRadius;
        float m_InnerRadius;
        float m_LabelRadius;
        float m_TextSize;
        Color m_MaskColor;
        Color m_FrameColor;
        float m_FrameLineWieght;
        string m_FontName;
        float m_Padding;
        DiagramCurveEnd m_CurveEnd;
        PointF m_Location;

        public static DiagramPieChart Create(List<double> Data, List<string> DataLabels, List<Color> DataFillColors, List<Color> DataLineColors, PointF Location,
        float OuterRadius, float InnerRadius, float LabelRadius, float LineWieght, float TextSize, Color textColor, Color maskColor, Color frameColor, float frameLineWieght,
        string fontName, float padding, DiagramCurveEnd CurveEnd)
        {
            var chart = new DiagramPieChart();
            chart.m_Data = Data;

            var backupFillColours = DiagramColour.GetColors(chart.m_Data.Count);

            for (var i = 0; i < chart.m_Data.Count; i++)
            {

                if (DataLabels.Count > i)
                {
                    chart.m_DataLabels.Add(DataLabels[i]);
                }
                else
                {
                    chart.m_DataLabels.Add(string.Empty);
                }
                if (DataFillColors.Count > i)
                {
                    chart.m_DataFillColors.Add(DataFillColors[i]);
                }
                else
                {
                    chart.m_DataFillColors.Add(backupFillColours[i]);

                }

                if (DataLineColors.Count > i)
                {
                    chart.m_DataLineColors.Add(DataLineColors[i]);
                }
                else
                {
                    if (i == 0)
                    {
                        chart.m_DataLineColors.AddRange(DiagramColour.GetColors(1, Diagram.DefaultColor));
                    }
                    else
                    {
                        chart.m_DataLineColors.AddRange(DiagramColour.GetColors(1, chart.m_DataLineColors[i - 1]));
                    }
                }
            }

            chart.m_Location = Location;

            chart.m_OuterRadius = OuterRadius;
            chart.m_InnerRadius = InnerRadius;
            chart.m_LabelRadius = LabelRadius;
            chart.m_TextSize = TextSize;
            chart._colour = textColor;
            chart.m_MaskColor = maskColor;
            chart.m_FrameColor = frameColor;
            chart.m_FrameLineWieght = frameLineWieght;
            chart.m_FontName = fontName;
            chart.m_Padding = padding;
            chart.m_CurveEnd = CurveEnd;
            chart._lineWeight = LineWieght;

            chart.UpdateCache();
            return chart;

        }

        public static DiagramPieChart Create(List<double> Data, List<string> DataLabels)
        {

            return Create(Data, DataLabels, DiagramColour.GetColors(Data.Count), DiagramColour.GetColors(Data.Count, Diagram.DefaultColor), Diagram.ConvertPoint(Point3d.Origin), 100,
                60, 130, Diagram.DefaultLineWeight, Diagram.DefaultTextScale, Diagram.DefaultColor, Color.Transparent, Diagram.DefaultColor, -1,
         Diagram.DefaultFontName, Diagram.DefaultPadding, null);

        }

        public static DiagramPieChart Create(List<double> Data, List<string> DataLabels, List<Color> DataFillColors, PointF Location,
      float OuterRadius, float InnerRadius, float LabelRadius)
        {

            return Create(Data, DataLabels, DataFillColors, DiagramColour.GetColors(Data.Count, Diagram.DefaultColor), Location, OuterRadius,
                InnerRadius, LabelRadius, Diagram.DefaultLineWeight, Diagram.DefaultTextScale, Diagram.DefaultColor, Color.Transparent, Diagram.DefaultColor, -1,
         Diagram.DefaultFontName, Diagram.DefaultPadding, null);

        }

        public override DiagramObject Duplicate()
        {
            var chart = new DiagramPieChart();
            chart.m_Data = m_Data;
            chart.m_DataLabels = m_DataLabels;
            chart.m_DataFillColors = m_DataFillColors;
            chart.m_Location = m_Location;
            chart.m_DataLineColors = m_DataLineColors;
            chart._lineWeight = _lineWeight;
            chart.m_OuterRadius = m_OuterRadius;
            chart.m_InnerRadius = m_InnerRadius;
            chart.m_LabelRadius = m_LabelRadius;
            chart.m_TextSize = m_TextSize;
            chart.m_MaskColor = m_MaskColor;
            chart.m_FrameColor = m_FrameColor;
            chart.m_FrameLineWieght = m_FrameLineWieght;
            chart.m_FontName = m_FontName;
            chart.m_Padding = m_Padding;
            chart.m_CurveEnd = m_CurveEnd;

            chart.UpdateCache();

            return chart;
        }

        private List<double> CalulateSegmentParameters()
        {
            var parameters = new List<double>();
            parameters.Add(0);

            double total = 0;
            for (var i = 0; i < m_Data.Count; i++)
            {
                total += m_Data[i];
            }

            double startingparam = 0;
            for (var i = 0; i < m_Data.Count; i++)
            {
                var paramAmount = (m_Data[i] / total) * (2 * Math.PI);
                parameters.Add(paramAmount + startingparam);
                startingparam += paramAmount;
            }

            return parameters;
        }

        public override List<DiagramObject> GenerateObjects()
        {

            var diagramObjects = new List<DiagramObject>();

            var rhinoPt = Diagram.ConvertPoint(m_Location);
            var mainCircle = new Circle(rhinoPt, m_OuterRadius);
            var innerCircle = new Circle(rhinoPt, m_InnerRadius);
            var labelCircle = new Circle(rhinoPt, m_LabelRadius);

            var parameters = this.CalulateSegmentParameters();

            for (var i = 0; i < m_Data.Count; i++)
            {
                var curves = new List<Curve>();

                var outerArc = new Arc(mainCircle, new Interval(parameters[i], parameters[i + 1]));
                var labelArc = new Arc(labelCircle, new Interval(parameters[i], parameters[i + 1]));

                curves.Add(outerArc.ToNurbsCurve());
                var innerPointStart = rhinoPt;
                var innerPointEnd = rhinoPt;
                if (m_InnerRadius > 0)
                {
                    var innerArc = new Arc(innerCircle, new Interval(parameters[i], parameters[i + 1]));

                    curves.Add(innerArc.ToNurbsCurve());
                    innerPointStart = innerArc.StartPoint;
                    innerPointEnd = innerArc.EndPoint;

                }

                var radius1 = new Line(innerPointStart, outerArc.StartPoint);
                var radius2 = new Line(innerPointEnd, outerArc.EndPoint);

                curves.Add(radius1.ToNurbsCurve());
                curves.Add(radius2.ToNurbsCurve());

                var joinedCurves = Curve.JoinCurves(curves);
                foreach (var joinedCurve in joinedCurves)
                {
                    diagramObjects.Add(DiagramHatch.Create(joinedCurve, m_DataFillColors[i], m_DataLineColors[i], _lineWeight));
                }

                if (m_DataLabels[i] == string.Empty) { continue; }

                var labelPointOnOuterArc = outerArc.ToNurbsCurve().PointAtNormalizedLength(0.5);
                var labelPointOnLabelArc = labelArc.ToNurbsCurve().PointAtNormalizedLength(0.5);
                var labelDummyLine = new Line(labelPointOnOuterArc, labelPointOnLabelArc);

                diagramObjects.Add(DiagramLabel.Create(m_DataLabels[i], Diagram.ConvertPoint(labelDummyLine.From), (float)labelDummyLine.Length, labelDummyLine.Direction, _colour, _lineWeight, m_TextSize, m_MaskColor, m_FrameColor, m_FrameLineWieght, m_FontName, m_Padding, m_CurveEnd));

            }
            return diagramObjects;
        }
    }
}
*/