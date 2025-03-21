namespace DiagramLibrary
{/*
    public interface IDiagramImageMaterial
    {
        Rhino.Display.DisplayMaterial DisplayMaterialCache { get; }
        Rhino.DocObjects.Material DocMaterialCache { get; }
        int DocMaterialCacheIndex { get; }
    }

    public class DiagramImageMaterial : IDiagramImageMaterial
    {
        public Rhino.Display.DisplayMaterial DisplayMaterialCache { get; }
        public Rhino.DocObjects.Material DocMaterialCache { get; }
        public int DocMaterialCacheIndex { get; }

        public DiagramImageMaterial()
        {
            this.DisplayMaterialCache = null;
            this.DocMaterialCache = null;
            this.DocMaterialCacheIndex = -1;
        }
    }

    public interface IDiagramImage : IDrawableDiagramObject
    {
        Image Image { get; }
        SizeF Size { get; }
        string ImagePath { get; }
        IDiagramImageMaterial DisplayMaterial { get; }
    }

    public class DiagramImage : IDiagramImage
    {
        public IDiagramLocation Location { get; }
        public IDiagramCurveAttributes CurveAttributes { get; }

        public IDiagramImageMaterial DisplayMaterial { get; }
        public Image Image { get; }

        public SizeF Size { get; }
        public string ImagePath { get; }

        /// <summary>
        /// Constructs a new Diagram Image
        /// </summary>

        public static DiagramImage Create(string imagePath, PointF Location, SizeF Size)
        {
            var diagramImage = new DiagramImage();
            diagramImage._colour = Diagram.DefaultColor;
            diagramImage._lineWeight = Diagram.DefaultLineWeight;
            diagramImage.ImagePath = imagePath;
            diagramImage.Image = Bitmap.FromFile(imagePath);
            diagramImage.Location = Location;
            if (Size == SizeF.Empty)
            {
                diagramImage.Size = diagramImage.Image.Size;
            }
            else
            {
                diagramImage.Size = Size;

            }
            diagramImage.DisplayMaterialCache = null;
            diagramImage.DocMaterialCache = null;
            diagramImage.DocMaterialCacheIndex = -1;

            return diagramImage;
        }

        public SizeF GetTotalSize()
        {
            throw new NotImplementedException();
        }

        IDiagramObject IDiagramObject.Duplicate()
        {
            throw new NotImplementedException();
        }

        public DiagramObject Duplicate()
        {
            var diagramImage = new DiagramImage();
            diagramImage._colour = _colour;
            diagramImage._lineWeight = _lineWeight;
            diagramImage.Image = this.Image;
            diagramImage.Location = this.Location;
            diagramImage.Size = this.Size;
            diagramImage.DisplayMaterialCache = null;
            diagramImage.DocMaterialCache = null;
            diagramImage.DocMaterialCacheIndex = -1;

            return diagramImage;
        }

        public PointF GetBoundingBoxLocation()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(this.Location.X, this.Location.Y, 0, this.Location.X + this.Size.Width, this.Location.Y + this.Size.Height, 0);
        }

        /// <inheritdoc />
        public void DrawBitmap(Graphics g)
        {
            // Drawn Upside Down as final image is flipped
            g.ScaleTransform(1, -1);
            g.DrawImage(this.Image, this.Location.X, -this.Location.Y - this.Size.Height, this.Size.Width, this.Size.Height);
            g.ResetTransform();
        }

        /// <inheritdoc />
        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xfrom, DrawState state)
        {

            var brep = this.GeneratePreviewGeometry();

            if (DisplayMaterialCache == null)
            {
                var texture = new Rhino.DocObjects.Texture();
                texture.FileName = this.ImagePath;

                DisplayMaterialCache = new Rhino.Display.DisplayMaterial();
                DisplayMaterialCache.SetBitmapTexture(texture, true);
            }
            pipeline.DrawBrepShaded(brep, DisplayMaterialCache);

            return;

        }

        /// <inheritdoc />
        private Brep GeneratePreviewGeometry()
        {
            var rec = new Rectangle3d(Plane.WorldXY, new Interval(this.Location.X, this.Location.X + this.Size.Width), new Interval(this.Location.Y, this.Location.Y + this.Size.Height));
            return Brep.CreateTrimmedPlane(Plane.WorldXY, rec.ToNurbsCurve());
        }

        /// <inheritdoc />
        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();

            var brep = this.GeneratePreviewGeometry();

            if (DocMaterialCache == null)
            {
                var texture = new Rhino.DocObjects.Texture();
                texture.FileName = this.ImagePath;

                DocMaterialCache = new Rhino.DocObjects.Material();
                DocMaterialCache.SetBitmapTexture(texture);
                DocMaterialCacheIndex = doc.Materials.Add(DocMaterialCache);
            }

            attr.MaterialSource = Rhino.DocObjects.ObjectMaterialSource.MaterialFromObject;

            attr.MaterialIndex = DocMaterialCacheIndex;

            outList.Add(doc.Objects.AddBrep(brep, attr));

            return outList;

        }
    }*/
}
