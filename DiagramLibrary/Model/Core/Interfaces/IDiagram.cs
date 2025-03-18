using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary.Core
{

    public interface IDiagram
    {
        IDiagramFrame Frame { get; }
        IDiagramInfo Info { get; }
        IDiagramObjectSet Objects { get; }
        IDiagramCoordinateSystem CoordinateSystem { get; }
        Diagram Duplicate();
        void AddObjects(Grasshopper.Kernel.GH_Component component, List<object> objs, double tolerance);
        Rectangle3d GetGeometryBoundingRectangle();
        BoundingBox GetGeometryBoundingBox();
        Size GetBoundingSize(float scale);

        Bitmap DrawBitmap(float scale);

        Report DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state);
        Report BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr, out List<Guid> guids);
    }
}