using DiagramLibrary;
using DiagramLibrary.Defaults;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Drawing;

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

            pManager.AddColourParameter("FillColour", "FClr", "Height in Pixels", GH_ParamAccess.item, DiagramDefaults.DefaultColor);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            this.GetAllValues(DA);
            var color = DiagramDefaults.DefaultColor;
            Brep brep = null;

            DA.GetData(0, ref brep);
            DA.GetData(1, ref color);

            if (brep == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Brep cannot be Null");
                return null;
            }

            var curveModifiers = this.GetFirstOrDefaultCurveModifier();

            var attributes = new DiagramCurveAttributes(curveModifiers.LineColors,
                (float)curveModifiers.LineWeight);

            var hatchAttributes = new DiagramHatchAttributes(color);

            var diagramCurves = DiagramHatch.CreateFromBrep(brep, attributes, hatchAttributes);

            if (diagramCurves == null || diagramCurves.Count == 0)
            {
                return null;
            }
            var bb = BoundingBox.Empty;

            var boundingBox = BoundingBox.Empty;
            foreach (var curve in diagramCurves)
            {
                bb.Union(curve.GetBoundingBox());

                boundingBox.Union(DiagramCoordinateSystem.ConvertPoint(curve.GetBoundingBoxLocation()));
            }

            var maxSize = new SizeF((float)(bb.Max.X - bb.Min.X), (float)(bb.Max.Y - bb.Min.Y));

            var frame = new DiagramFrame((int)Math.Ceiling(maxSize.Width), (int)Math.Ceiling(maxSize.Height), Color.Transparent, 0, Color.Transparent);

            var title = DiagramDefaults.DefaultTitle;

            var location = new DiagramLocation(boundingBox.Min);

            var diagram = new Diagram(frame, title, location);

            foreach (var diagramHatch in diagramCurves)
            {
                diagram.Objects.Add(diagramHatch);
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