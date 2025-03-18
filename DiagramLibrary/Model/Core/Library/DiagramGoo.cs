using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;

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
            return this.DuplicateDiagram();
        }

        public DiagramGoo DuplicateDiagram()
        {
            return new DiagramGoo(this.Value == null ? new Diagram() : this.Value.Duplicate());
        }

        public override bool IsValid
        {
            get
            {
                if (this.Value == null) { return false; }
                return true;
            }
        }

        public override string ToString()
        {
            if (this.Value == null)
                return "Null Diagram";
            else
                return this.Value.ToString();
        }

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = Guid.Empty;
            var diagram = this.Value;
            if (diagram == null) { return false; }

            diagram.BakeRhinoPreview(doc.ModelAbsoluteTolerance, Transform.ZeroTransformation, DrawState.Normal, doc, att, out var obj_ids);
            obj_guid = obj_ids[0];
            return true;
        }

        public override string TypeName => ("Diagram");

        public override string TypeDescription => ("Defines a Diagram");
    }
}
