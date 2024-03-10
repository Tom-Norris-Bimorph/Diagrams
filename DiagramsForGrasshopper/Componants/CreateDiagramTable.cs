using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using DiagramLibrary;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramTable : DiagramComponentWithModifiers
    { 
        /// <summary>
        /// Initializes a new instance of the CreateDiagramTable class.
        /// </summary>
    public CreateDiagramTable()
          : base("CreateDiagramTable", "DTable",
                "A componant to create Tables to be used in diagrams",
              "Display", "Diagram")
        {
            Modifiers.Add(new TextModifiers(true, true, true, true, false, false, false, false));
            Modifiers.Add(new CurveModifiers(true, true,false,false));
        }


        protected override void RegisterInputStartingParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Data", "Data", "Data as a string DataTree each branch is a row", GH_ParamAccess.tree);
            pManager.AddPointParameter("Location", "Loc", "Location for text", GH_ParamAccess.item, new Point3d(0, 0, 0));
            pManager.HideParameter(1);
            pManager.AddNumberParameter("CellWidths", "CWdths", "List of Widths for each column, first value will be the default width", GH_ParamAccess.list, 100);
            pManager.AddNumberParameter("CellHeight", "CHghts", "List of Heights for each Row, first value will be the default height", GH_ParamAccess.list, 30);
            pManager.AddColourParameter("Background Colour", "BClr", "Back Colour", GH_ParamAccess.item, Color.Transparent);
            pManager.AddColourParameter("Header Colour", "HClr", "Header Colour", GH_ParamAccess.item, Color.Gray);
            pManager.AddBooleanParameter("Flip Direction", "Flip", "Flip the Direction of the table",GH_ParamAccess.item,false);
        }


     
     

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            pManager.AddGenericParameter("CSV", "CSV", "CSV format of the table data", GH_ParamAccess.item);
        }




        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            GetAllValues( DA);

           Color bgClr = Color.Transparent;
            Point3d pt = new Point3d(0, 0, 0);
              List<double> width = new List<double>();
            List<double> height = new List<double>();
            GH_Structure<GH_String> data;
            bool flip = false;
            Color headerColour = Color.Gray;


            if (!DA.GetDataTree(0, out data))
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "data cannot be Empty");
                return null;
            }
         
            DA.GetData(1, ref pt);
            DA.GetDataList(2,  width);
            DA.GetDataList(3, height);
            DA.GetData(4, ref bgClr);
            DA.GetData(5, ref headerColour);
            DA.GetData(6, ref flip);


            if (data == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "data cannot be null");
                return null;
            }

            if (width.Count == 0)
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cell Widths need at least on value");
                return null;
            }
            if (height.Count == 0)
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cell Heights need at least on value");
                return null;
            }


            PointF location = Diagram.ConvertPoint(pt);
            TextModifiers textModifiers = GetFirstOrDefaultTextModifier();
            CurveModifiers curveModifiers = GetFirstOrDefaultCurveModifier();

          

            DiagramTable diagramTable = DiagramTable.Create(data,width,height, (float )textModifiers.TextScale, location, curveModifiers.LineColors, (float)curveModifiers.LineWeight, textModifiers.Font,(float)textModifiers.TextPadding,textModifiers.TextJustification,headerColour,flip);
            SizeF size = diagramTable.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, bgClr,0,Color.Transparent,location);
            diagram.AddDiagramObject( diagramTable );


           

            DA.SetData(2, diagramTable.ToCSV());
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
                return DiagramsForGrasshopper.Properties.Resources.TableIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b0ba788e-7715-436d-8d7c-ef12e1f1fce3"); }
        }

        public override GH_Exposure Exposure

        {

            get { return GH_Exposure.secondary; }

        }
    }
}