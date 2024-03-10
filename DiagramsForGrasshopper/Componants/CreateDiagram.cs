using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using DiagramLibrary;
using System.Drawing;
using System.Linq;


// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace DiagramsForGrasshopper
{
    public class CreateDiagram : DiagramComponent
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CreateDiagram()
          : base("CreateDiagram", "CreateDiagram",
              "A componant to create diagrams from mutiple Diagram Objects",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Height", "Hght", "Diagram Height in Pixels", GH_ParamAccess.item, -1);
            pManager.AddIntegerParameter("Width", "Wdth", "Diagram Width in Pixels", GH_ParamAccess.item, -1);
            pManager.AddGenericParameter("DiagramObjects", "DObjs", "Diagram objects or Rhino Geometry To Add to Diagram", GH_ParamAccess.list);
    
            pManager.AddGenericParameter("Title", "Title", "Optional Diagram Title", GH_ParamAccess.item);
            this.Params.Input[3].Optional = true;
            pManager.AddColourParameter("Colour", "BgClr", "Optional Background Colour", GH_ParamAccess.item, System.Drawing.Color.Transparent);
            pManager.AddNumberParameter("Frame Line Weight", "FLW", "Optional Frame Line Weight", GH_ParamAccess.item, 0);
            pManager.AddColourParameter("Frame Colour", "FClr", "Optional Frame Line Colour", GH_ParamAccess.item, Diagram.DefaultColor);


        }

      


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            int width = -1;
            int height = -1;
            List<Object> objs = new List<Object>();
            Grasshopper.Kernel.Types.IGH_Goo titleObj = null;

            Color clr = System.Drawing.Color.Transparent;
            Color fclr = Diagram.DefaultColor;
            double frameLineWeight = 0;

            DA.GetData(0, ref width);
            DA.GetData(1, ref height);
            
            DA.GetDataList(2, objs);
            DA.GetData(3, ref titleObj);
            DA.GetData(4, ref clr);

            DA.GetData(5, ref frameLineWeight);
            DA.GetData(6, ref Diagram.DefaultColor);



            DiagramText titleDiagram = null;
            if (titleObj != null)
            {
                if (titleObj.GetType() == typeof(Grasshopper.Kernel.Types.GH_String))
                {
                    titleObj.CastTo(out string diagramTitleString);
                    titleDiagram = DiagramText.Create(diagramTitleString, PointF.Empty, Diagram.DefaultColor, -1f, TextJustification.BottomLeft, Color.Transparent, Diagram.DefaultColor, 0, Diagram.DefaultFontName, new SizeF(-1, -1), 3, TextJustification.BottomLeft);

                }
                else
                {
                    try
                    {
                        titleObj.CastTo(out Diagram diagramTitleDiagram);

                        titleDiagram = (diagramTitleDiagram.Objects.Where(x => x.GetType() == typeof(DiagramText)).FirstOrDefault() as DiagramText).Duplicate() as DiagramText;

                    }
                    catch (Exception)
                    {


                    }
                    
                }
            }



            
          
           


            Diagram diagram = Diagram.Create(width, height, titleDiagram, clr, (float)frameLineWeight, fclr);
            diagram.AddObjects(this,objs, GH_Component.DocumentTolerance());





         return diagram;

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return DiagramsForGrasshopper.Properties.Resources.DiagramIcon;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("67321aae-32da-4e18-8efc-e0d94f763c78"); }
        }
    }
}
