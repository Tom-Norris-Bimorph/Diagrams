using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramDimention : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the DiagramDimention class.
        /// </summary>
        public CreateDiagramDimention()
          : base("DiagramDimention", "DDimention",
              "A componant to create Dimentions to be used in diagrams",
             "Display", "Diagram")
        {
            Modifiers.Add(new TextModifiers(true, true, true,false,false,true,false, false));
            Modifiers.Add(new CurveModifiers(true, true, true, true));
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point1", "Pt1", "Point1", GH_ParamAccess.item);
            pManager.AddPointParameter("Point2", "Pt1", "Point1", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset", "Offset", "Offset from Object", GH_ParamAccess.item, 10);
            pManager.AddTextParameter("TextOverride", "TOvrd", "Text to override of the dimention", GH_ParamAccess.item, string.Empty);
            pManager.AddTextParameter("Suffix", "Sfx", "Suffix of the dimention", GH_ParamAccess.item, string.Empty);
            pManager.AddIntegerParameter("Round", "Rnd", "The number of decimals the text will round to", GH_ParamAccess.item, 2);

        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            this.GetAllValues(DA);
            double offset = 0;
            Point3d p1 = Point3d.Unset;
            Point3d p2 = Point3d.Unset;
            string suffix = string.Empty;
            string overrideText = string.Empty;
            int round = 2;

         

            DA.GetData(0, ref p1);
            DA.GetData(1, ref p2);
            DA.GetData(2, ref offset);
            DA.GetData(3, ref overrideText);
            DA.GetData(4, ref suffix);
            DA.GetData(5, ref round);
        
          
            if (p1 == Point3d.Unset)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Point1 cannot be Null");
                return null;
            }

            if (p2 == Point3d.Unset)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Point2 cannot be Null");
                return null;
            }



            TextModifiers textModifiers = this.GetFirstOrDefaultTextModifier();
            CurveModifiers curveModifiers = this.GetFirstOrDefaultCurveModifier();



            DiagramDimention diagramDimention = DiagramDimention.Create(p1, p2, textModifiers.TextColor, (float)curveModifiers.LineWeight, suffix, overrideText, textModifiers.TextBackgroundColor, (float)textModifiers.TextScale, textModifiers.Font, (float)textModifiers.TextPadding, round, DiagramCurveEnd.DefaultDimentionCurveEnd(1, textModifiers.TextColor, (float)curveModifiers.LineWeight), (float)offset);

            if (curveModifiers.StartingCurveEnd != null) {
                diagramDimention.AddCurveEnds(curveModifiers.StartingCurveEnd, null);
            }
            if (curveModifiers.EndingCurveEnd != null)
            {
                diagramDimention.AddCurveEnds(null,curveModifiers.EndingCurveEnd);
            }


            SizeF size = diagramDimention.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramDimention.GetBoundingBoxLocation());
            diagram.AddDiagramObject(diagramDimention);


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
                return DiagramsForGrasshopper.Properties.Resources.DimIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("890c976e-2a3e-4a9e-9cb6-99624e768edf"); }
        }
    }
}
