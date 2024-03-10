using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramPointLabel : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramLabel class.
        /// </summary>  
        public CreateDiagramPointLabel()
          : base("CreateDiagramPointLabel", "DPtLabel",
              "A componant to create a small label to be used in diagrams",
            "Display", "Diagram")
        {
            Modifiers.Add(new TextModifiers(true, true, false, false, false, true, true, true));
          

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "Txt", "Text as a string", GH_ParamAccess.item);
                      pManager.AddPointParameter("LabelPoint", "LPt", "The point on the target for the label", GH_ParamAccess.item, Point3d.Origin);
            pManager.HideParameter(1);
      
        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            GetAllValues( DA);

            string text = "";
            Point3d pt = new Point3d(0, 0, 0);
            
            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
          

            if (text == "")
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Text cannot be Empty");
                return null;
            }

            TextModifiers textModifiers = GetFirstOrDefaultTextModifier();



            PointF location = Diagram.ConvertPoint(pt);

            DiagramPointLabel diagramLabel = DiagramPointLabel.Create(text, location, textModifiers.TextColor, (float)textModifiers.TextScale, textModifiers.TextBackgroundColor, textModifiers.TextBorderColor, (float)textModifiers.TextBorderLineweight, textModifiers.Font,  (float)textModifiers.TextPadding);

            SizeF size = diagramLabel.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramLabel.DiagramText.GetAnchorCompensatedPoint(size));
            diagram.AddDiagramObject(diagramLabel);
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
                return DiagramsForGrasshopper.Properties.Resources.PointIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("bae04aa7-f9d5-4a55-8871-aa5954130fa2"); }
        }
    }
}