using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class Diagram : IDiagram
    {
        private readonly DiagramObjectConverter _converter = new DiagramObjectConverter();
        public IDiagramInfo Info { get; }

        public IDiagramFrame Frame { get; }

        public IDiagramObjectSet Objects { get; }

        public IDiagramCoordinateSystem CoordinateSystem { get; }

        public int Width => this.Frame.Width;
        public int Height => this.Frame.Height;

        public Diagram(IDiagramFrame frame, IDiagramTitle title, IDiagramLocation location)
        {
            this.Objects = new DiagramObjectSet();

            this.Info = new DiagramInfo(title);

            this.Frame = frame;

            this.CoordinateSystem = new DiagramCoordinateSystem(location);
        }

        public Diagram() : this(DiagramDefaults.DefaultFrame, DiagramDefaults.DefaultTitle, new PointF(0, 0)) { }

        public Diagram Duplicate()
        {
            var diagram = new Diagram(this.Frame, this.Info.Title, this.CoordinateSystem.Location);
            foreach (var diagramObject in this.Objects)
            {
                diagram.Objects.Add(diagramObject.Duplicate());
            }

            return diagram;
        }

        public void Convert(Grasshopper.Kernel.GH_Component component,
            List<object> objects, double tolerance)
        {
            this.Objects.AddRange(_converter.Convert(component, objects, tolerance));
        }

        public Rectangle3d GetGeometryBoundingRectangle()
        {
            var plane = Plane.WorldXY;

            if (this.Width > 0 && this.Height > 0)
                return new Rectangle3d(plane, this.Width, this.Height);

            var bb = BoundingBox.Unset;
            foreach (var item in this.Objects)
            {
                bb.Union(item.GetBoundingBox());
            }

            double width = this.Width;
            double height = this.Height;

            if (this.Width <= 0)
            {
                width = bb.Max.X - bb.Min.X;
            }

            if (height <= 0)
            {
                height = bb.Max.Y - bb.Min.Y;
            }

            return new Rectangle3d(plane, width, height);

        }

        public BoundingBox GetGeometryBoundingBox()
        {
            return this.GetGeometryBoundingRectangle().ToNurbsCurve().GetBoundingBox(false);
        }

        public Size GetBoundingSize(float scale)
        {
            var bb = this.GetGeometryBoundingRectangle();

            return new Size((int)Math.Ceiling(bb.Width * scale), (int)Math.Ceiling(bb.Height * scale));
        }

        private DiagramHatchRectangle GetBackground()
        {
            var bbr = this.GetGeometryBoundingRectangle();
            bbr.Transform(Transform.Translation(this.CoordinateSystem.Location.Point.X, this.CoordinateSystem.Location.Point.Y, 0));

            return DiagramHatchRectangle.Create(bbr, this.Frame.BackgroundColour, this.Frame.FrameColour, this.Frame.FrameLineWeight);

        }

        /// <summary>
        /// be careful all the Y dimentions need to be be subtracted from the the hieght at this is drawn upside down
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public Bitmap DrawBitmap(float scale)
        {
            var size = this.GetBoundingSize(scale);

            if (size.Width < 1) size.Width = 1;

            if (size.Height < 1) size.Height = 1;

            var bitmap = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TranslateTransform(-this.CoordinateSystem.Location.Point.X, -this.CoordinateSystem.Location.Point.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
                graphics.ScaleTransform(scale, scale);
                graphics.FillRectangle(Brushes.White, new RectangleF(0, 0, this.Width, this.Height));// text displays badly without this, if no background is set
                this.GetBackground().DrawBitmap(graphics);
                foreach (var diagramObject in this.Objects)
                {

                    if (diagramObject is IDrawableDiagramObject drawableDiagramObject == false) continue;

                    try
                    {
                        drawableDiagramObject.DrawBitmap(graphics);
                    }
                    catch (Exception ex)
                    {
                        // component.AddRuntimeMessage(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning, "GH Canvas: An Object was Skipped When Drawing: " + ex.Message);
                    }
                }
            }

            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bitmap;
        }

        public Report DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, DrawState state)
        {
            var report = new Report();

            var locationPoint = this.CoordinateSystem.Location.Point;

            if (locationPoint != PointF.Empty)
            {
                transform = Transform.Multiply(transform, Transform.Translation(new Vector3d(-locationPoint.X, -locationPoint.Y, 0)));
            }

            var background = this.GetBackground();

            background.DrawRhinoPreview(pipeline, tolerance, transform, state);

            if (this.Info.Title != null)
            {
                var pt = new PointF(0, this.Height);
                var title = this.Info.Title.TitleText;
                title.Location.Point = pt;
                if (title.TextAttributes.TextSize < 0)
                {
                    title.TextAttributes.TextSize = this.Width / 20;
                }

                title.DrawRhinoPreview(pipeline, tolerance, transform, state);
            }

            foreach (var diagramObject in this.Objects)
            {
                if (diagramObject is IDrawableDiagramObject drawableDiagramObject == false) continue;
                try
                {
                    drawableDiagramObject.DrawRhinoPreview(pipeline, tolerance, transform, state);
                }
                catch (Exception ex)
                {

                    report.AddMessage("Rhino Preview: An Object was Skipped When Drawing: " + ex.Message, Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning);
                }
            }

            return report;

        }

        public Report BakeRhinoPreview(double tolerance, Transform transform, DrawState state
            , Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr, out List<Guid> guids)
        {
            guids = new List<Guid>();
            var report = new Report();

            var locationPoint = this.CoordinateSystem.Location.Point;

            if (locationPoint != PointF.Empty)
            {
                transform = Transform.Multiply(transform, Transform.Translation(new Vector3d(-locationPoint.X, -locationPoint.Y, 0)));
            }

            var background = this.GetBackground();
            guids.AddRange(background.BakeRhinoPreview(tolerance, transform, state, doc, attr));

            if (this.Info.Title.TitleText != null)
            {
                var titlePoint = new PointF(0, this.Height);
                var title = this.Info.Title.TitleText;
                title.Location = titlePoint;
                if (title.TextSize < 0)
                {
                    title.TextSize = this.Width / 20;
                }

                guids.AddRange(title.BakeRhinoPreview(tolerance, transform, state, doc, attr));
            }

            foreach (var diagramObject in this.Objects)
            {
                if (diagramObject is IDrawableDiagramObject drawableDiagramObject == false) continue;
                try
                {
                    guids.AddRange(drawableDiagramObject.BakeRhinoPreview(tolerance, transform, state, doc, attr));
                }
                catch (Exception ex)
                {

                    report.AddMessage("Rhino Bake: An Object was Skipped When Drawing: " + ex.Message, Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning);
                }
            }

            return report;

        }
    }
}

