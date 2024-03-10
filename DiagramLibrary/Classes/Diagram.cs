using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class Diagram
    {
        public static Color DefaultColor = Color.Black;
        public static float DefaultLineWeight = 1f;
        public static float DefaultTextScale = 10f;
        public static string DefaultFontName = "Arial";
        public static float DefaultPadding = 3f;
        public static Color SelectedColor = Color.FromArgb(128, Color.ForestGreen);

        private PointF m_Location;
        private int m_Width;
        private int m_Height;
        private DiagramText m_Title;
        private string m_TitleFont = Diagram.DefaultFontName;

        private Color m_BackgroundColour;
        private Color m_FrameColour;
        private float m_FrameLineWeight;


        private List<DiagramObject> m_Objects;


        public static PointF ConvertPoint(Point3d pt)
        {
            return new PointF((float)pt.X, (float)pt.Y);
        }

        public static Point3d ConvertPoint(PointF pt)
        {
            return new Point3d(pt.X, pt.Y, 0);
        }


        static public string LibraryVersion()
        {
            return typeof(Diagram).Assembly.GetName().Version.ToString();
        }



        public DiagramText Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }


        public List<DiagramObject> Objects
        {
            get { return m_Objects; }

        }



        public Diagram Duplicate()
        {
            Diagram diagram = new Diagram();
            diagram.m_Width = m_Width;
            diagram.m_Height = m_Height;
            diagram.m_Objects = m_Objects;
            diagram.m_FrameColour = m_FrameColour;
            diagram.Title = m_Title;
            diagram.m_TitleFont = m_TitleFont;
            diagram.m_FrameColour = m_FrameColour;
            diagram.m_FrameLineWeight = m_FrameLineWeight;


            return diagram;
        }

        public Diagram() { }


        public static Diagram Create(int width, int height, DiagramText title, Color backgroundColour, float frameLineWeight, Color frameColour)
        {
            return Create(width, height, title, backgroundColour, frameLineWeight, frameColour, new PointF(0, 0));
        }


        //Do not use the location to set the diagram location in the RhinoPreview, use the xfrom in the DrawinRhinoPreview method, the diagram location is used to compensate for single object diagram's locations
        public static Diagram Create(int width, int height, DiagramText title, Color backgroundColour, float frameLineWeight, Color frameColour, PointF location)
        {
            Diagram diagram = new Diagram();


            diagram.m_Width = width;

            diagram.m_Height = height;
       


            diagram.m_Objects = new List<DiagramObject>();
            diagram.m_Title = title;
            diagram.m_BackgroundColour = backgroundColour;
            diagram.m_FrameColour = frameColour;
            diagram.m_FrameLineWeight = frameLineWeight;


            diagram.m_Location = location;
            return diagram;
        }

        public void AddObjects(Grasshopper.Kernel.GH_Component component, List<object> objs, double tolernace)
        {
            for (int i = 0; i < objs.Count; i++)
            {



                try
                {
                    var goo = objs[i] as Grasshopper.Kernel.Types.IGH_GeometricGoo;
                    goo.CastTo(out GeometryBase geoBase);
                    AddRhinoObject(component, objs[i], tolernace);



                }
                catch (Exception)
                {
                    AddDiagramObjectFromGoo(component, objs[i]);

                }



            }

        }


        public void AddDiagramObject(DiagramObject obj)
        {

            m_Objects.Add(obj);
        }


        public void AddDiagramObjectFromGoo(Grasshopper.Kernel.GH_Component component, object obj)
        {
            if (obj == null)
            {
                component.AddRuntimeMessage(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning, "Cannont add null object to diagram");
                return;
            }

            var goo = obj as Grasshopper.Kernel.Types.IGH_Goo;


            goo.CastTo(out Diagram diagram);
            if (diagram == null) { return; }
            if (diagram.m_Objects.Count == 0) { return; }
            m_Objects.AddRange(diagram.m_Objects);



        }




        public void AddRhinoObject(Grasshopper.Kernel.GH_Component component, object obj, double tolernace)
        {
            if (obj == null) { return; }


            // if (obj is GeometryBase == false) {                return;            }

            var goo = obj as Grasshopper.Kernel.Types.IGH_GeometricGoo;
            goo.CastTo(out GeometryBase geoBase);

            switch (geoBase.ObjectType)
            {
                case Rhino.DocObjects.ObjectType.None:
                    break;
                case Rhino.DocObjects.ObjectType.Point:
                    break;
                case Rhino.DocObjects.ObjectType.PointSet:
                    break;
                case Rhino.DocObjects.ObjectType.Curve:
                    goo.CastTo(out Curve crv);

                    DiagramCurve dCurve = DiagramCurve.Create(crv, DefaultColor, DefaultLineWeight);
                    m_Objects.Add(dCurve);
                    break;
                case Rhino.DocObjects.ObjectType.Surface:
                    goo.CastTo(out Surface srf);

                    AddBrep(tolernace, srf.ToBrep());
                    break;
                case Rhino.DocObjects.ObjectType.Brep:

                    goo.CastTo(out Brep brep);
                    AddBrep(tolernace, brep);

                    break;
                case Rhino.DocObjects.ObjectType.Mesh:
                    break;
                case Rhino.DocObjects.ObjectType.Light:
                    break;
                case Rhino.DocObjects.ObjectType.Annotation:
                    break;
                case Rhino.DocObjects.ObjectType.InstanceDefinition:
                    break;
                case Rhino.DocObjects.ObjectType.InstanceReference:
                    break;
                case Rhino.DocObjects.ObjectType.TextDot:
                    break;
                case Rhino.DocObjects.ObjectType.Grip:
                    break;
                case Rhino.DocObjects.ObjectType.Detail:
                    break;
                case Rhino.DocObjects.ObjectType.Hatch:

                    break;
                case Rhino.DocObjects.ObjectType.MorphControl:
                    break;
                case Rhino.DocObjects.ObjectType.SubD:
                    break;
                case Rhino.DocObjects.ObjectType.BrepLoop:
                    break;
                case Rhino.DocObjects.ObjectType.PolysrfFilter:
                    break;
                case Rhino.DocObjects.ObjectType.EdgeFilter:
                    break;
                case Rhino.DocObjects.ObjectType.PolyedgeFilter:
                    break;
                case Rhino.DocObjects.ObjectType.MeshVertex:
                    break;
                case Rhino.DocObjects.ObjectType.MeshEdge:
                    break;
                case Rhino.DocObjects.ObjectType.MeshFace:
                    break;
                case Rhino.DocObjects.ObjectType.Cage:
                    break;
                case Rhino.DocObjects.ObjectType.Phantom:
                    break;
                case Rhino.DocObjects.ObjectType.ClipPlane:
                    break;
                case Rhino.DocObjects.ObjectType.Extrusion:
                    goo.CastTo(out Extrusion ext);

                    AddBrep(tolernace, ext.ToBrep());
                    break;
                case Rhino.DocObjects.ObjectType.AnyObject:
                    break;
                default:
                    break;
            }
        }

        private void AddBrep(double tolernace, Brep brep)
        {
            m_Objects.AddRange(DiagramFilledCurve.CreateFromBrep(brep, DefaultColor, DefaultColor, DefaultLineWeight));
        }



        public Rectangle3d GetGeometryBoundingRectangle()
        {
            Plane pl = Plane.WorldXY;

            if (m_Width <= 0 || m_Height <= 0)
            {
                BoundingBox bb = BoundingBox.Unset;
                foreach (var item in m_Objects)
                {
                    bb.Union(item.GetBoundingBox());
                }

                double width = m_Width;
                double height = m_Height;

                if (m_Width <= 0)
                {
                    width = bb.Max.X - bb.Min.X;
                }

                if (height <= 0)
                {
                    height = bb.Max.Y - bb.Min.Y;
                }


                return new Rectangle3d(pl, width, height);


            }
            else
            {
                return new Rectangle3d(pl, m_Width, m_Height);
            }


        }


        public BoundingBox GetGeometryBoundingBox()
        {
            return this.GetGeometryBoundingRectangle().ToNurbsCurve().GetBoundingBox(false);

        }

        public Size GetBoundingSize(float scale)
        {
            Rectangle3d bb = GetGeometryBoundingRectangle();

            return new Size((int)Math.Ceiling(bb.Width* scale), (int)Math.Ceiling(bb.Height* scale));
        }


        private DiagramFilledRectangle GetBackground()
        {

            Rectangle3d bbr = this.GetGeometryBoundingRectangle();
            bbr.Transform(Transform.Translation(m_Location.X, m_Location.Y, 0));

            return DiagramFilledRectangle.Create(bbr, m_BackgroundColour, m_FrameColour, m_FrameLineWeight);

        }

        public Bitmap DrawBitmap(float scale) // be careful all the Y dimentions need to be be subtracted from the the hieght at this is drawn upside down
        {
            //TODO DrawBitmap needs to pass on the scale and draw accordingly
            Size size = GetBoundingSize(scale);

            if (size.Width < 1)
            {
                size.Width = 1;
            }

            if (size.Height < 1)
            {
                size.Height = 1;
            }



            Bitmap btm = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);



            using (var graphics = Graphics.FromImage(btm))
            {
                graphics.TranslateTransform(-m_Location.X, -m_Location.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
                graphics.ScaleTransform(scale, scale);
                graphics.FillRectangle(Brushes.White, new RectangleF(0, 0, this.m_Width, this.m_Height));// text displays badly without this, if no background is set
                GetBackground().DrawBitmap( graphics);
                foreach (DiagramObject obj in m_Objects)
                {
                    try
                    {
                        obj.DrawBitmap( graphics);
                    }
                    catch (Exception ex)
                    {

                       // component.AddRuntimeMessage(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning, "GH Canvas: An Object was Skipped When Drawing: " + ex.Message);
                    }
                   

                }


            }

            btm.RotateFlip(RotateFlipType.RotateNoneFlipY);


            return btm;

        }


        public  Report DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
            Report report = new Report();
           
            if (m_Location != PointF.Empty)
            {
               
                xform = Transform.Multiply(xform, Transform.Translation(new Vector3d(-m_Location.X, -m_Location.Y, 0)));

            }

            GetBackground().DrawRhinoPreview(pipeline, tolerance, xform, state);

            if (m_Title != null)
            {
                PointF pt = new PointF(0, this.m_Height);
                DiagramText title = m_Title;
                title.Location = pt;
                if (title.TextSize < 0)
                {
                    title.TextSize = this.m_Width / 20;
                }

                title.DrawRhinoPreview( pipeline, tolerance, xform, state);
            }

            foreach (DiagramObject obj in m_Objects)
            {

                try
                {
                    obj.DrawRhinoPreview( pipeline, tolerance, xform, state);
                }
                catch (Exception ex)
                {

                    report.AddMessage("Rhino Preview: An Object was Skipped When Drawing: " + ex.Message, Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning);
                    }

                

            }

            return report;

        }

        public Report BakeRhinoPreview( double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr, out List<Guid> guids)
        {
            guids = new List<Guid>();
            Report report = new Report();

            if (m_Location != PointF.Empty)
            {

                xform = Transform.Multiply(xform, Transform.Translation(new Vector3d(-m_Location.X, -m_Location.Y, 0)));

            }

            guids.AddRange( GetBackground().BakeRhinoPreview(tolerance, xform, state,doc, attr));

            if (m_Title != null)
            {
                PointF pt = new PointF(0, this.m_Height);
                DiagramText title = m_Title;
                title.Location = pt;
                if (title.TextSize < 0)
                {
                    title.TextSize = this.m_Width / 20;
                }

                guids.AddRange(title.BakeRhinoPreview( tolerance, xform, state, doc, attr));
            }

            foreach (DiagramObject obj in m_Objects)
            {

                try
                {
                    guids.AddRange(obj.BakeRhinoPreview( tolerance, xform, state, doc, attr));
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
