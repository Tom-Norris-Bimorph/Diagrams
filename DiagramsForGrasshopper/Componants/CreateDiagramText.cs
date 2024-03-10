using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramText : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramText class.
        /// </summary>
        public CreateDiagramText()
          : base("CreateDiagramText", "DText",
              "Create Text for a Diagram",
               "Display", "Diagram")
        {
            Modifiers.Add(new TextModifiers(true, true, true, true, true, true, true, true));
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "Txt", "Text as a string", GH_ParamAccess.item);
            pManager.AddPointParameter("Location", "Loc", "Location for text", GH_ParamAccess.item, Point3d.Origin);
            pManager.HideParameter(1);
            pManager.AddIntegerParameter("Anchor", "Anc",
               "Text Anchor 0: Bottom Left, 1: Bottom Center, 2: Bottom Right \n 3: Middle Left, 4: Middle Center, 5: Middle Right \n 6: Top Left, 7: Top Center, 8: Top Right",
               GH_ParamAccess.item, 0);

            pManager.AddNumberParameter("Max Width", "Wdth", "Maximum Width, Set to less than 0 to ignore ", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Max Height", "Hght", "Maximum Height, Set to less than 0 to ignore", GH_ParamAccess.item, -1);

                 

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
            int anchorInt = 0;
            double width = -1;
            double height = -1;
       

            DA.GetData(0, ref text);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref anchorInt);
            DA.GetData(3, ref width);
            DA.GetData(4, ref height);
          

            if (text == "")
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Text cannot be Empty");
                return null;
            }

            TextJustification anchor = TextJustification.BottomLeft;

            switch (anchorInt)
            {
                case 0:
                    anchor = TextJustification.BottomLeft;
                    break;
                case 1:
                    anchor = TextJustification.BottomCenter;
                    break;
                case 2:
                    anchor = TextJustification.BottomRight;
                    break;
                case 3:
                    anchor = TextJustification.MiddleLeft;
                    break;
                case 4:
                    anchor = TextJustification.MiddleCenter;
                    break;
                case 5:
                    anchor = TextJustification.MiddleRight;
                    break;
                case 6:
                    anchor = TextJustification.TopLeft;
                    break;
                case 7:
                    anchor = TextJustification.TopCenter;
                    break;
                case 8:
                    anchor = TextJustification.TopRight;
                    break;
                default:
                    // Use default values
                    anchor = TextJustification.BottomLeft;
                    break;
            }
          

            TextModifiers textModifiers = GetFirstOrDefaultTextModifier();
            CurveModifiers curveModifiers = GetFirstOrDefaultCurveModifier();

            PointF location = Diagram.ConvertPoint(pt);

            DiagramText diagramText = DiagramText.Create(text, location, textModifiers.TextColor, (float)textModifiers.TextScale, anchor, textModifiers.TextBackgroundColor, textModifiers.TextBorderColor, (float)textModifiers.TextBorderLineweight, textModifiers.Font, new SizeF((float)width, (float)height), (float)textModifiers.TextPadding, textModifiers.TextJustification);

            SizeF size = diagramText.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramText.GetAnchorCompensatedPoint(size));
            diagram.AddDiagramObject(diagramText);
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
                return DiagramsForGrasshopper.Properties.Resources.TextIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1c44a5e7-7fdd-41e0-8d7d-4b4c27b77a12"); }
        }
    }
}