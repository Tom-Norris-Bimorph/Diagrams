using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramLibrary
{
    public class DiagramGoo : GH_Goo<Diagram>, IGH_BakeAwareData
    {

        public DiagramGoo()
        {
            this.Value = new Diagram();
        }


        public DiagramGoo(Diagram diagram)
        {
            if (diagram == null)
                diagram = new Diagram();
            this.Value = diagram;
        }


        public override IGH_Goo Duplicate()
        {
            return DuplicateDiagram();
        }

        public DiagramGoo DuplicateDiagram()
        {
            return new DiagramGoo(Value == null ? new Diagram() : Value.Duplicate());
        }

        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return true;
            }
        }

        public override string ToString()
        {
            if (Value == null)
                return "Null Diagram";
            else
                return Value.ToString();
        }

        
        /*
        public void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            obj_ids = new List<Guid>();
            var diagram = Value;
            if (diagram == null) { return; }


            diagram.BakeRhinoPreview(doc.ModelAbsoluteTolerance, Transform.ZeroTransformation, DrawState.Normal, doc, doc.CreateDefaultAttributes(), out obj_ids);

        }

        public void BakeGeometry(RhinoDoc doc, ObjectAttributes att, List<Guid> obj_ids)
        {
            obj_ids = new List<Guid>();
            var diagram = Value;
            if (diagram == null) { return; }


            diagram.BakeRhinoPreview(doc.ModelAbsoluteTolerance, Transform.ZeroTransformation, DrawState.Normal, doc, att, out obj_ids);

        }
        */
        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = Guid.Empty;
             var diagram = Value;
            if (diagram == null) { return false; }


            diagram.BakeRhinoPreview(doc.ModelAbsoluteTolerance, Transform.ZeroTransformation, DrawState.Normal, doc, att, out List<Guid> obj_ids);
            obj_guid = obj_ids[0];
            return true;
        }

       // public bool IsBakeCapable => true;

        public override string TypeName
        {
            get { return ("Diagram"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a Diagram"); }
        }

       
    }
}
