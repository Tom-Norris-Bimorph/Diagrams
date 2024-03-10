using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using DiagramLibrary;

namespace DiagramsForGrasshopper
{
    public class RhinoDiagram : DiagramComponent
    {



        /// <summary>
        /// Initializes a new instance of the RhinoDiagram class.
        /// </summary>
        public RhinoDiagram()
          : base("RhinoDiagram", "RhinoDiagram",
              "Description",
                "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddGenericParameter("Diagram", "DObjs", "Image or Diagram to display inside this componant", GH_ParamAccess.item);
        

            pManager.AddTransformParameter("Transform", "Xfrom", "A transformation for the diagram", GH_ParamAccess.item);
            this.Params.Input[1].Optional = true;
        }

       
    

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            Diagram diagram = null;
            if (!DA.GetData(0, ref diagram))
            {
                return null;
            }

                       
            if (diagram == null)
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error,"Diagram cannot be null");
                return null;
            }

            DA.GetData(1, ref m_Xform);
            return diagram;


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
                return DiagramsForGrasshopper.Properties.Resources.RhinoIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1754a08f-2742-4038-807c-8d93f7bfe4a0"); }
        }



        public override GH_Exposure Exposure

        {

            get { return GH_Exposure.tertiary; }

        }

    }
}
