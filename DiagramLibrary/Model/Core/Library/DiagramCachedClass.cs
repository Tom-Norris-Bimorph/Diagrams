using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public abstract class DiagramCachedClass : DiagramObject
    {
        protected List<DiagramObject> m_ObjectCache = new List<DiagramObject>();

        public override Color Colour
        {
            get { return _colour; }
            set { _colour = value; this.UpdateCache(); }
        }

        public override float LineWeight
        {
            get { return _lineWeight; }
            set { _lineWeight = value; this.UpdateCache(); }
        }

        public virtual void UpdateCache()
        {
            m_ObjectCache = this.GenerateObjects();
        }

        public abstract List<DiagramObject> GenerateObjects();

        public override void DrawBitmap(Graphics g)
        {

            for (var i = 0; i < m_ObjectCache.Count; i++)
            {
                m_ObjectCache[i].DrawBitmap(g);
            }
        }



        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {

            for (var i = 0; i < m_ObjectCache.Count; i++)
            {
                m_ObjectCache[i].DrawRhinoPreview(pipeline, tolerance, xform, state);
            }
            return;

        }

        public override List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            var outList = new List<Guid>();
            for (var i = 0; i < m_ObjectCache.Count; i++)
            {
                outList.AddRange(m_ObjectCache[i].BakeRhinoPreview(tolerance, xform, state, doc, attr));
            }
            return outList;

        }

        public override BoundingBox GetBoundingBox()
        {
            var bbox = BoundingBox.Empty;

            for (var i = 0; i < m_ObjectCache.Count; i++)
            {
                bbox.Union(m_ObjectCache[i].GetBoundingBox());
            }
            return bbox;
        }
    }
}
