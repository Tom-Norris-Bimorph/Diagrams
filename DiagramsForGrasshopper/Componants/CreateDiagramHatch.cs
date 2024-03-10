using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramHatch : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramHatch class.
        /// </summary>
        public CreateDiagramHatch()
          : base("CreateDiagramSolidHatch", "DSHatch",
              "A componant to create Hatches or Filled Curves to be used in diagrams",
              "Display", "Diagram")
        {
            Modifiers.Add(new CurveModifiers(true, true, false, false));
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "Brep", "Height in Pixels", GH_ParamAccess.item);
   
            pManager.AddColourParameter("FillColour", "FClr", "Height in Pixels", GH_ParamAccess.item, Diagram.DefaultColor);
       
        
        }

     
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            this.GetAllValues(DA);
            Color clr = Diagram.DefaultColor;
          Brep brep = null;
      

            DA.GetData(0, ref brep);
            DA.GetData(1, ref clr);
         


            if (brep == null) {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Brep cannot be Null");
                return null;
            }

            CurveModifiers curveModifiers = this.GetFirstOrDefaultCurveModifier();

            List<DiagramFilledCurve> diagramCurves = DiagramFilledCurve.CreateFromBrep(brep, clr, curveModifiers.LineColors, (float)curveModifiers.LineWeight);



            if (diagramCurves == null || diagramCurves.Count == 0) {
                return null;
            }
            BoundingBox bb = BoundingBox.Empty;

            BoundingBox locationBB = BoundingBox.Empty;
            for (int i = 0; i < diagramCurves.Count; i++)
            {
                bb.Union(diagramCurves[i].GetBoundingBox());

                locationBB.Union(Diagram.ConvertPoint(diagramCurves[i].GetBoundingBoxLocation()));
               
            }

            SizeF maxSize = new SizeF((float)(bb.Max.X - bb.Min.X), (float)(bb.Max.Y - bb.Min.Y));

            Diagram diagram = Diagram.Create((int)Math.Ceiling(maxSize.Width), (int)Math.Ceiling(maxSize.Height), null, Color.Transparent, 0, Color.Transparent, Diagram.ConvertPoint(locationBB.Min));

            for (int i = 0; i < diagramCurves.Count; i++)
            {
                diagram.AddDiagramObject(diagramCurves[i]);
            }

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
                return DiagramsForGrasshopper.Properties.Resources.FilledCurveIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0d59cdbf-27fd-47c6-b22a-e3f17bea5072"); }
        }
    }
}