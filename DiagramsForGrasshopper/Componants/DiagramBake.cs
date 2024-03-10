using System;
using System.Collections.Generic;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class DiagramBake : GH_Component
    {
        private Transform m_Xform = Transform.ZeroTransformation;
        /// <summary>
        /// Initializes a new instance of the DiagramBake class.
        /// </summary>
        public DiagramBake()
          : base("DiagramBake", "DBake",
              "A componant to Bake Diagram Objects",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Diagram", "DObjs", "Diagram to display inside this componant", GH_ParamAccess.item);
            pManager.AddBooleanParameter("No Fill", "NoFll", "Optional, Removes all fills from the diagrams", GH_ParamAccess.item, false);
            pManager.AddTransformParameter("Transform", "Xfrom", "A transformation for the diagram", GH_ParamAccess.item);
            this.Params.Input[2].Optional = true;
            pManager.AddBooleanParameter("Bake", "Bake", "Bakes the diagram", GH_ParamAccess.item, false);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool runBake = false;

            bool noFill = false;
            Diagram diagram = null;


            if (!DA.GetData(0, ref diagram))
            {
                return;
            }

            if (diagram == null)
            {
                return;
            }

            DA.GetData(1, ref noFill);
            DA.GetData(2, ref m_Xform);
            DA.GetData(3, ref runBake);

            if (runBake) {
                DrawState state = DrawState.Normal;
                 if (noFill) {
                   state = DrawState.NoFills;
                }
                Rhino.RhinoDoc doc = Rhino.RhinoDoc.ActiveDoc;


                diagram.BakeRhinoPreview( GH_Component.DocumentTolerance(), m_Xform, state, doc, doc.CreateDefaultAttributes(), out List<Guid> guids);
            }

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return DiagramsForGrasshopper.Properties.Resources.BakeIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7fa3e356-2741-46b7-b3e1-779cf94630b7"); }
        }

        public override GH_Exposure Exposure

        {

            get { return GH_Exposure.tertiary; }

        }
    }
}