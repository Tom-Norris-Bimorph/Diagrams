using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
   public  class DiagramImage : DiagramObject
    {
    
        private Image m_Image;
        private PointF m_Location;
        private SizeF m_Size;
        private string m_ImagePath;
        private Rhino.Display.DisplayMaterial m_DisplayMaterialCache;
        private Rhino.DocObjects.Material m_DocMaterialCache;
        private int m_DocMaterialCacheIndex;


        public static DiagramImage Create(string imagePath, PointF Location, SizeF Size)
        {
            DiagramImage diagramImage = new DiagramImage();
            diagramImage.m_Colour = Diagram.DefaultColor;
            diagramImage.m_LineWeight = Diagram.DefaultLineWeight;
            diagramImage.m_ImagePath = imagePath;            
            diagramImage.m_Image = Bitmap.FromFile(imagePath); 
            diagramImage.m_Location = Location;
            if (Size == SizeF.Empty)
            {
                diagramImage.m_Size = diagramImage.m_Image.Size;
            }
            else {
                diagramImage.m_Size = Size;

            }
            diagramImage.m_DisplayMaterialCache = null;
            diagramImage.m_DocMaterialCache = null;
            diagramImage.m_DocMaterialCacheIndex = -1;

            return diagramImage;
        }

        public override DiagramObject Duplicate()
        {
            DiagramImage diagramImage = new DiagramImage();
            diagramImage.m_Colour = m_Colour;
            diagramImage.m_LineWeight = m_LineWeight;
            diagramImage.m_Image = m_Image;
            diagramImage.m_Location = m_Location;
            diagramImage.m_Size = m_Size;
            diagramImage.m_DisplayMaterialCache = null;
            diagramImage.m_DocMaterialCache = null;
            diagramImage.m_DocMaterialCacheIndex = -1;

            return diagramImage;
        }

        public override BoundingBox GetBoundingBox() {
            return new BoundingBox(m_Location.X,m_Location.Y,0, m_Location.X+ m_Size.Width, m_Location.Y + m_Size.Height, 0);
        }


        public override void DrawBitmap(Graphics g)
        {

           // Drawn Upside Down as final image is flipped
            g.ScaleTransform(1, -1);
            g.DrawImage(m_Image, m_Location.X, -m_Location.Y- m_Size.Height, m_Size.Width, m_Size.Height);
            g.ResetTransform();
        }

       

        public override void DrawRhinoPreview( Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xfrom, DrawState state)
        {
            
            Brep brep = GeneratePreviewGeometry();

            if (m_DisplayMaterialCache == null)
                {
                    var texture = new Rhino.DocObjects.Texture();
                    texture.FileName = m_ImagePath;

                    m_DisplayMaterialCache = new Rhino.Display.DisplayMaterial();
                    m_DisplayMaterialCache.SetBitmapTexture(texture, true);
                }
                pipeline.DrawBrepShaded(brep, m_DisplayMaterialCache);
            
           

            return;

        }

        private Brep GeneratePreviewGeometry() {
            Rectangle3d rec = new Rectangle3d(Plane.WorldXY, new Interval(m_Location.X, m_Location.X + m_Size.Width), new Interval(m_Location.Y, m_Location.Y + m_Size.Height));
            return Brep.CreateTrimmedPlane(Plane.WorldXY, rec.ToNurbsCurve());
        }

        public override List<Guid> BakeRhinoPreview( double tolerance, Transform xfrom, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            List<Guid> outList = new List<Guid>();

            Brep brep = GeneratePreviewGeometry();


            
                if (m_DocMaterialCache == null)
                {
                    var texture = new Rhino.DocObjects.Texture();
                    texture.FileName = m_ImagePath;

                    m_DocMaterialCache = new Rhino.DocObjects.Material();
                    m_DocMaterialCache.SetBitmapTexture(texture);
                    m_DocMaterialCacheIndex = doc.Materials.Add(m_DocMaterialCache);
                }


               
                attr.MaterialSource = Rhino.DocObjects.ObjectMaterialSource.MaterialFromObject;

                attr.MaterialIndex = m_DocMaterialCacheIndex;

                outList.Add(doc.Objects.AddBrep(brep, attr));
            


            return outList;

        }


    }
}
