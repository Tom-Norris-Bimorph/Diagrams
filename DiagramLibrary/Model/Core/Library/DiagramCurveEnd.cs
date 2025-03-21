using DiagramLibrary.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{

    public class DiagramCurveEnd : IDiagramCurveEnd
    {

        public IBaseCurveDiagramObject Object { get; }

        public Point3d BasePoint { get; }
        public Vector3d BaseDirection { get; }
        public bool Flipped { get; private set; }

        public DiagramCurveEnd(IBaseCurveDiagramObject diagramObject, Point3d location, Vector3d rotation, bool flipped)
        {
            this.Object = diagramObject.Duplicate();
            this.BasePoint = location;
            this.BaseDirection = rotation;
            this.Flipped = flipped;
        }

        public IDiagramCurveEnd DuplicateCurveEnd()
        {
            return this.Duplicate() as DiagramCurveEnd;
        }

        public IDiagramObject Duplicate()
        {
            return new DiagramCurveEnd(this.Object, this.BasePoint, this.BaseDirection, this.Flipped);
        }

        public void Flip()
        {
            this.Flipped = !this.Flipped;
        }

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform transform, DrawState state, Point3d location, Vector3d rotation)
        {

            var flipCorrectedDirection = this.BaseDirection;
            if (this.Flipped)
            {
                flipCorrectedDirection.Reverse();
            }

            var positionedObject = this.Object.SetLocationAndDirectionForDrawing(this.BasePoint, flipCorrectedDirection, location, rotation);
            if (positionedObject != null)
            {
                positionedObject.DrawRhinoPreview(pipeline, tolerance, transform, state);
            }
        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Point3d location, Vector3d rotation, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();

            var flipCorrectedDirection = this.BaseDirection;
            if (this.Flipped)
            {
                flipCorrectedDirection.Reverse();
            }

            var positionedObject = this.Object.SetLocationAndDirectionForDrawing(this.BasePoint, flipCorrectedDirection, location, rotation);
            if (positionedObject != null)
            {
                outList.AddRange(positionedObject.BakeRhinoPreview(tolerance, transform, state, doc, attr));
            }
            return outList;
        }

        public void DrawBitmap(Graphics g, Point3d location, Vector3d rotation)
        {
            var positionedObject = this.Object.SetLocationAndDirectionForDrawing(this.BasePoint, this.BaseDirection, location, rotation);
            if (positionedObject != null)
            {
                positionedObject.DrawBitmap(g);
            }
        }
    }
}
