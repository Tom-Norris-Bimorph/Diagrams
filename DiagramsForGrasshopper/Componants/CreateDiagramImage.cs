using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramImage : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the CreateDiagramImage class.
        /// </summary>
        public CreateDiagramImage()
          : base("CreateDiagramImage", "DImage",
              "A componant to create Images to be used in diagrams",
              "Display", "Diagram")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Image", "Img", "File path to the image", GH_ParamAccess.item);
         //   this.Params.Input[0].ObjectChanged += CreateDiagramImage_ObjectChanged;
            pManager.AddPointParameter("Location", "Loc", "Point locatin for the image", GH_ParamAccess.item, new Point3d(0,0,0));
            pManager.HideParameter(1);
            pManager.AddNumberParameter("Width", "Wdth", "Width in Pixels, Optional: set to less than 0 to use the image's width", GH_ParamAccess.item,-1);
            pManager.AddNumberParameter("Height", "Hght", "Height in Pixels, Optional: set to less than 0 to use the image's height", GH_ParamAccess.item, -1);
        }

     

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            this.GetAllValues(DA);

            string path = "";
            Point3d pt = Point3d.Unset;
              double width = -1;
            double height = -1;

            DA.GetData(0, ref path);
            DA.GetData(1, ref pt);
            DA.GetData(2, ref width);
            DA.GetData(2, ref height);




            Image btm = Bitmap.FromFile(path);

            if (width < 0) {
                width = btm.Width;
            }

            if (height < 0) {
                height = btm.Height;
            }


            PointF location = Diagram.ConvertPoint(pt);

            DiagramImage diagramImage = DiagramImage.Create(path, location, new SizeF((float)width, (float)height));
            SizeF size = diagramImage.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, location);
            diagram.AddDiagramObject(diagramImage);


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
                return DiagramsForGrasshopper.Properties.Resources.ImageIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5b4628dd-e32b-461b-aeb4-8da00c6746a8"); }
        }
    }
}