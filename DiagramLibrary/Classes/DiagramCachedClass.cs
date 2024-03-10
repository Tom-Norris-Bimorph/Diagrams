using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
   public abstract class  DiagramCachedClass : DiagramObject
    {
        protected List<DiagramObject> m_ObjectCache = new List<DiagramObject>();

        public override Color Colour
        {
            get { return m_Colour; }
            set { m_Colour = value; UpdateCache(); }
        }

        public override float LineWeight
        {
            get { return m_LineWeight; }
            set { m_LineWeight = value; UpdateCache(); }
        }

        public virtual void UpdateCache()
        {
            this.m_ObjectCache = GenerateObjects();
        }

        public abstract List<DiagramObject> GenerateObjects();


        public override void DrawBitmap( Graphics g)
        {

            for (int i = 0; i < this.m_ObjectCache.Count; i++)
            {
                this.m_ObjectCache[i].DrawBitmap( g);
            }
        }

       


        public override void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {

            for (int i = 0; i < m_ObjectCache.Count; i++)
            {
                m_ObjectCache[i].DrawRhinoPreview(pipeline, tolerance, xform, state);
            }
            return;

        }


        public override List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {
            List<Guid> outList = new List<Guid>();
            for (int i = 0; i < m_ObjectCache.Count; i++)
            {
                outList.AddRange(m_ObjectCache[i].BakeRhinoPreview(tolerance, xform, state, doc, attr));
            }
            return outList;

        }



        public override BoundingBox GetBoundingBox()
        {
            BoundingBox bbox = BoundingBox.Empty;

            for (int i = 0; i < m_ObjectCache.Count; i++)
            {
                bbox.Union(m_ObjectCache[i].GetBoundingBox());
            }
            return bbox;
        }

    }
}
