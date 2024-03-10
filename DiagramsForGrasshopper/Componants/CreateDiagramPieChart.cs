using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;




namespace DiagramsForGrasshopper
{
    public class CreateDiagramPieChart : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the CreatePieChart class.
        /// </summary>
        public CreateDiagramPieChart()
          : base("CreateDiagramPieChart", "DPie", "A componant to create Pie Charts to be used in diagrams",
                "Display", "Diagram")
        {
            Modifiers.Add(new TextModifiers(true, true, true, true, false, false, false, false));
            Modifiers.Add(new CurveModifiers(true, true,true,false));
        }
        
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Data", "Data", "A list of data to repsent in a pie chart", GH_ParamAccess.list);
            pManager.AddTextParameter("DataLabels", "DLbl", "An optional list of Text labels for each of the data items", GH_ParamAccess.list,new List<string> { string.Empty});
          
            pManager.AddColourParameter("DataColour", "DClr", "An optional list of Colours for each of the data items", GH_ParamAccess.list, DiagramColour.GetColors(1));
            pManager.AddColourParameter("DataLineColour", "DLClr", "An optional list of Colours for each of the data items", GH_ParamAccess.list, DiagramColour.GetColors(1));
            pManager.AddPointParameter("Location", "Loc", "The point on the target for the label", GH_ParamAccess.item, Point3d.Origin);
            pManager.HideParameter(1);
            pManager.AddNumberParameter("OuterRadius", "ORad", "Offset from Label Point", GH_ParamAccess.item, 100);
            pManager.AddNumberParameter("InnerRadius", "IRad", "Offset from Label Point", GH_ParamAccess.item, 30);
           pManager.AddNumberParameter("LabelRadius", "LRad", "Offset from Label Point", GH_ParamAccess.item, 130);
         

        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {

            GetAllValues(DA);

            string text = "";
            Point3d pt = Point3d.Origin;
           
            Vector3d direction = new Vector3d(1, 1, 0);

            List<double> data = new List<double>();
            List<string> dataLabels = new List<string>();
            List<Color> dataFillColors = new List<Color>();
            List<Color> dataLineColors = new List<Color>();

            double outerRadius = 100;
            double innerRadius = 30;
            double labelRadius = 130;

           

            DA.GetDataList(0, data);
            DA.GetDataList(1,  dataLabels);
            DA.GetDataList(2,  dataFillColors);
            DA.GetDataList(3, dataLineColors);
            DA.GetData(4, ref pt);
            DA.GetData(5, ref outerRadius);
            DA.GetData(6, ref innerRadius);
            DA.GetData(7, ref labelRadius);


   
            if (data.Count == 0)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "data cannot be Empty");
                return null;
            }


            PointF location = Diagram.ConvertPoint(pt);

            TextModifiers textModifiers = GetFirstOrDefaultTextModifier();
            CurveModifiers curveModifiers = GetFirstOrDefaultCurveModifier();

            DiagramPieChart diagramPie = DiagramPieChart.Create(data, dataLabels, dataFillColors, dataLineColors, location,
        (float) outerRadius, (float)innerRadius, (float)labelRadius, (float) curveModifiers.LineWeight, (float)textModifiers.TextScale, textModifiers.TextColor,  textModifiers.TextBackgroundColor,  textModifiers.TextBorderColor, (float) textModifiers.TextBorderLineweight,
        textModifiers.Font, (float)textModifiers.TextPadding, curveModifiers.StartingCurveEnd);



            SizeF size = diagramPie.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramPie.GetBoundingBoxLocation());
            diagram.AddDiagramObject(diagramPie);
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
                return DiagramsForGrasshopper.Properties.Resources.PieIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("01b20595-71ef-48b8-a343-0c62b8bb46b4"); }
        }

        public override GH_Exposure Exposure

        {

            get { return GH_Exposure.secondary; }

        }
    }
}