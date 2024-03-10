using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;


namespace DiagramLibrary
{
    public abstract class NoDrawDiagramObject : DiagramObject
    {

        public abstract override  DiagramObject Duplicate();

        //These are required for DiagramObject so we can store this information as a diagram eg CurveEnds which are only valid as when applied to a curve and all the heaviy lift is done by the curve, CurveEnds should never draw themselves or any of the below methods

        public override void DrawBitmap( Graphics g)
        {
            return; //skip
        }

        public override void DrawRhinoPreview(DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {
          
            return; // skip
        }

        public override List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {

            return new List<Guid>(); // skip
        }


        public override BoundingBox GetBoundingBox()
        {
            return BoundingBox.Empty;
        }

        public override PointF GetBoundingBoxLocation()
        {
            return PointF.Empty;
        }




    }
}
