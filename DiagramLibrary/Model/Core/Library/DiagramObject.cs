namespace DiagramLibrary;

/*
public abstract class DiagramObject : IDiagramObject
{
    protected Color _colour;

    protected float _lineWeight;

    public virtual Color Colour
    {
        get => _colour;
        set => _colour = value;
    }

    public virtual float LineWeight
    {
        get => _lineWeight;
        set => _lineWeight = value;
    }

   

    public virtual PointF GetBoundingBoxLocation()
    {
        var bbox = this.GetBoundingBox();
        return Diagram.ConvertPoint(bbox.Min);
    }

    public SizeF GetTotalSize()
    {
        var bbox = this.GetBoundingBox();
        return new SizeF((float)(bbox.Max.X - bbox.Min.X), (float)(bbox.Max.Y - bbox.Min.Y));
    }

    public abstract BoundingBox GetBoundingBox();

    public abstract void DrawBitmap(Graphics g);

    public abstract void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state);

    public abstract List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr);

    public abstract DiagramObject Duplicate();

}*/

