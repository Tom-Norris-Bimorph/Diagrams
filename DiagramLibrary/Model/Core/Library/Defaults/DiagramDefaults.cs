using DiagramLibrary.Core;
using DiagramLibrary.Text;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary.Defaults
{
    public class DiagramDefaults
    {
        public static Color DefaultColor = Color.Black;
        public const float DefaultLineWeight = 1f;
        public const float DefaultTextHeight = 10f;
        public const string DefaultFontName = "Arial";
        public const float DefaultPadding = 3f;
        public static Color SelectedColor = Color.FromArgb(128, Color.ForestGreen);
        public static SizeF DefaultWrapSize = new SizeF(100, 100);
        public static IDiagramCurveAttributes DefaultDiagramAttributes = new DiagramCurveAttributes(DefaultColor, DefaultLineWeight);

        public static IDiagramTextJustification DefaultDiagramJustification = new DiagramTextJustification(TextJustification.Bottom);
        public static IDiagramTextAnchor DefaultAnchor = new DiagramTextAnchor(TextJustification.Bottom);

        public const double Tolerance = 0.01;

        public static IDiagramLocation DefaultLocation =
            new DiagramLocation(new PointF(0, 0));


        public static IDiagramCurveEnd DefaultCurveEnd = new DiagramCurveEnd(new DiagramCurveCollection(new List<Curve>(), DiagramDefaults.DefaultDiagramAttributes), Point3d.Origin, Vector3d.YAxis, false);

        public static IDiagramFrame DefaultFrame = new DiagramFrame(new Rectangle3d(Plane.WorldXY, 100, 100), DiagramDefaults.DefaultDiagramAttributes);
        public static IDiagramTitle DefaultTitle = new DiagramTitle(new DiagramText(string.Empty, DefaultLocation, new DiagramTextAttributes(),))

        public static IDiagramCurveEnd DefaultDimensionCurveEnd(double scale)
        {
            var curves = new List<Curve>
            {
                new Line(Point3d.Origin, Vector3d.YAxis, 10 * scale).ToNurbsCurve(),
                new Line(Point3d.Origin, Vector3d.XAxis, 5 * scale).ToNurbsCurve(),
                new Line(Point3d.Origin, Vector3d.XAxis, -5 * scale).ToNurbsCurve(),
                new Line(Point3d.Origin, new Vector3d(1, 1, 0), 5 * scale).ToNurbsCurve(),
                new Line(Point3d.Origin, new Vector3d(1, 1, 0), -5 * scale).ToNurbsCurve()
            };

            var crvCollection = new DiagramCurveCollection(curves, DiagramDefaults.DefaultDiagramAttributes);
            var curveEnd = new DiagramCurveEnd(crvCollection, Point3d.Origin, Vector3d.YAxis, false);
            return curveEnd;

        }

    }
}