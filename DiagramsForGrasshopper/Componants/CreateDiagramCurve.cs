using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramCurve : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramCurve class.
        /// </summary>
        public CreateDiagramCurve()
          : base("CreateDiagramCurve", "DCrv",
              "A componant to create Curves to be used in diagrams",
              "Display", "Diagram")
        {
            Modifiers.Add(new CurveModifiers(true, true, true, true));
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Crv", "The Curve", GH_ParamAccess.item);
 

        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            this.GetAllValues(DA);
        
            Curve crv = null;


            DA.GetData(0, ref crv);
           
            if (crv == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error,"Curve cannot be Null");
                return null;
            }

            CurveModifiers curveModifiers = this.GetFirstOrDefaultCurveModifier();
           

            DiagramCurve diagramCurve = DiagramCurve.Create(crv, curveModifiers.LineColors, (float)curveModifiers.LineWeight);
            diagramCurve.AddCurveEnds(curveModifiers.StartingCurveEnd, curveModifiers.EndingCurveEnd);

            SizeF size = diagramCurve.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent,0,Color.Transparent, diagramCurve.GetBoundingBoxLocation());
            diagram.AddDiagramObject(diagramCurve);


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
                return DiagramsForGrasshopper.Properties.Resources.CurveIcon;   
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("eeb4bbf4-584b-4d37-8896-5d61b8cdd82c"); }
        }
    }
}