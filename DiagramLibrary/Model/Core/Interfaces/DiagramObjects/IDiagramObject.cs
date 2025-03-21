using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary.Core
{

    /// <summary>
    /// Base Interface for all Diagram Objects
    /// </summary>
    public interface IDiagramObject
    {
        IDiagramLocation Location { get; }

        SizeF GetTotalSize();
        PointF GetBoundingBoxLocation();

        BoundingBox GetBoundingBox();

        IDiagramObject Duplicate();
    }

    public interface IDrawableDiagramObject : IDiagramObject
    {

        IDiagramCurveAttributes CurveAttributes { get; }

        void DrawBitmap(Graphics g);

        void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance,
            Transform xform, DrawState state);

        List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state,
            Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr);

    }
}
