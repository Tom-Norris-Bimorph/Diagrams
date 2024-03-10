using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    abstract public class DiagramObject 
    {
         protected Color m_Colour;
      
         protected float m_LineWeight;

    


        public virtual Color Colour { 
            get { return m_Colour; }
            set { m_Colour = value;  }
        }

        public virtual float LineWeight 
        {
            get { return m_LineWeight; }
            set { m_LineWeight = value; }
        }

       

        public DiagramObject() { }

   
       

        public Pen GetPen() {
            return new Pen(m_Colour, m_LineWeight);
        }


        public virtual PointF GetBoundingBoxLocation()
        {
            BoundingBox bbox = this.GetBoundingBox();
            return Diagram.ConvertPoint(bbox.Min);
        }


        public SizeF GetTotalSize() {
            BoundingBox bbox = this.GetBoundingBox();
            return new SizeF((float)(bbox.Max.X - bbox.Min.X), (float)(bbox.Max.Y - bbox.Min.Y));
        }

        public abstract BoundingBox GetBoundingBox();

        public abstract void DrawBitmap(Graphics g);

        public abstract void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state);

        public abstract List<Guid> BakeRhinoPreview(double tolerance, Transform xform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr);

        public abstract DiagramObject Duplicate();

      

    }
}
