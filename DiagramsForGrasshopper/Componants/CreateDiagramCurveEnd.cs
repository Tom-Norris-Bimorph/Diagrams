using System;
using System.Collections.Generic;
using System.Drawing;
using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramsForGrasshopper
{
    public class CreateDiagramCurveEnd : DiagramComponentWithModifiers
    {
        /// <summary>
        /// Initializes a new instance of the DiagramCurveEnd class.
        /// </summary>
        public CreateDiagramCurveEnd()
          : base("CreateDiagramCurveEnd", "DCrvEnd",
              "A componant to create Curve Ends to be used in diagrams Curve Elements",
          "Display", "Diagram")
        {
            Modifiers.Add(new CurveModifiers(true, true, false, false));
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputStartingParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "Crvs", "The Curve", GH_ParamAccess.list);
            pManager.AddPointParameter("Point", "Pt", "Pivot point of the Curve End, this is the point which will be put on the target Curve's end point, Default is the Origin", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddVectorParameter("Direction", "Dir", "This is the direction of the Curve End which will be aligned with the tangent of the Target Curve's end point, Default is the Y - Axis", GH_ParamAccess.item,Vector3d.YAxis);
            pManager.AddBooleanParameter("Flipped", "Flip", "Flip the direction of the Curve End", GH_ParamAccess.item, true);

        }



        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        public override Diagram DiagramSolveInstance(IGH_DataAccess DA)
        {
            this.GetAllValues(DA);

       
            List<Curve> crvs = new List<Curve>();
            Point3d point = Point3d.Origin;
           Vector3d direction = Vector3d.YAxis;
            bool flipped = true;


            DA.GetDataList(0, crvs);
            DA.GetData(1, ref point);
            DA.GetData(2, ref direction);
            DA.GetData(3, ref flipped);
       

            if (crvs.Count == 0)
            {
                 this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error,"Curve cannot be Null");
                return null;
            }

            CurveModifiers curveModifiers = this.GetFirstOrDefaultCurveModifier();

         
            DiagramCurveCollection diagramCurveCollection = DiagramCurveCollection.Create(crvs,curveModifiers.LineColors, (float)curveModifiers.LineWeight);

            DiagramCurveEnd diagramCurveEnd = new DiagramCurveEnd(diagramCurveCollection, point, direction, flipped);

           


            SizeF size = diagramCurveCollection.GetTotalSize();
            Diagram diagram = Diagram.Create((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), null, Color.Transparent, 0, Color.Transparent, diagramCurveCollection.GetBoundingBoxLocation());
            diagram.AddDiagramObject(diagramCurveEnd);


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
                return DiagramsForGrasshopper.Properties.Resources.CurveEndsIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ef7532ad-fac2-44c6-9695-d14adb58dd30"); }
        }
    }
}