using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramLabel : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the DiagramLabel class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the CreateDiagramLabel class.
        /// </summary>
        public CreateDiagramLabel()
          : base("CreateDiagramLabel", "DLabel",
              "A componant to create small text labels to be used in diagrams",
            "Display", "Diagram")
        {
            Modifiers.Add(new TextModifiers(true, true, true, false, true, true, true, true));
            Modifiers.Add(new CurveModifiers(true, false, true, false));

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "Txt", "Text as a string", GH_ParamAccess.item);
            pManager.AddPointParameter("LabelPoint", "LPt", "The point on the target for the label", GH_ParamAccess.item, Point3d.Origin);
            pManager.HideParameter(1);
            pManager.AddNumberParameter("Offset", "Offset", "Offset from Label Point", GH_ParamAccess.item, 10);
            pManager.AddVectorParameter("Direction", "Dir", "Direction for label offset", GH_ParamAccess.item, new Vector3d(1, 1, 0));
            pManager.HideParameter(3);
           
        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {

            this.GetAllValues(DA);

            string text = "";
            Point3d pt = new Point3d(0, 0, 0);
            double offset = 10;
            Vector3d direction = new Vector3d(1, 1, 0);

            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref offset);
            DA.GetData(3, ref direction);
            

            if (text == "")
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Text cannot be Empty");
                return null;
            }


            TextModifiers textModifiers = this.GetFirstOrDefaultTextModifier();
            CurveModifiers curveModifiers  = this.GetFirstOrDefaultCurveModifier();


            PointF location = Diagram.ConvertPoint(pt);
       

            DiagramLabel diagramLabel = DiagramLabel.Create(text, location, (float)offset, direction, textModifiers.TextColor, (float)curveModifiers.LineWeight, (float)textModifiers.TextScale, textModifiers.TextBackgroundColor, textModifiers.TextBorderColor, (float)textModifiers.TextBorderLineweight, textModifiers.Font, (float)textModifiers.TextPadding, curveModifiers.StartingCurveEnd);


            SizeF size = diagramLabel.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramLabel.GetBoundingBoxLocation());
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
                return DiagramsForGrasshopper.Properties.Resources.LabelIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5bfac095-5802-43ce-a5c2-8b0e9afef8c1"); }
        }
    }
}