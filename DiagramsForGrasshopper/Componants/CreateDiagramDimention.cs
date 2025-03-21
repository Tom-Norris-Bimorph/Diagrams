using DiagramLibrary;
using DiagramLibrary.Defaults;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Drawing;

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
            Modifiers.Add(new TextModifiers(true, true, true, false, false, true, false, false));
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
            var startPoint = Point3d.Unset;
            var endPoint = Point3d.Unset;
            var suffix = string.Empty;
            var overrideText = string.Empty;
            var roundToPlaces = 2;

            DA.GetData(0, ref startPoint);
            DA.GetData(1, ref endPoint);
            DA.GetData(2, ref offset);
            DA.GetData(3, ref overrideText);
            DA.GetData(4, ref suffix);
            DA.GetData(5, ref roundToPlaces);

            if (startPoint == Point3d.Unset)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Point1 cannot be Null");
                return null;
            }

            if (endPoint == Point3d.Unset)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Point2 cannot be Null");
                return null;
            }

            var textModifiers = this.GetFirstOrDefaultTextModifier();
            var curveModifiers = this.GetFirstOrDefaultCurveModifier();

            var line = new LineCurve(startPoint, endPoint);

            var dimensionAttributes =
                new DimensionAttributes((float)offset, roundToPlaces, suffix, overrideText);

            var startCurveEnd = curveModifiers.StartingCurveEnd ?? DiagramDefaults.DefaultDimensionCurveEnd(1);


            var curveAttributes = new DiagramCurveAttributes(curveModifiers.LineColors, (float)curveModifiers.LineWeight);

            var justification = new DiagramTextJustification(TextJustification.Center);

            var anchor = new DiagramTextAnchor(TextJustification.Center);

            var textAttributes = new DiagramTextAttributes(curveAttributes, justification, anchor,, textModifiers.TextColor, textModifiers.TextBackgroundColor,
                textModifiers.Font, (float)textModifiers.TextScale, (float)textModifiers.TextPadding);

            var diagramDimension = new DiagramDimension(line,
                dimensionAttributes, startCurveEnd, curveAttributes, textAttributes);

            var size = diagramDimension.GetTotalSize();

            var frame = new DiagramFrame((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), Color.Transparent, 0, Color.Transparent);

            var title = DiagramDefaults.DefaultTitle;

            var location = new DiagramLocation(diagramDimension.GetBoundingBoxLocation());

            var diagram = new Diagram(frame, title, location);

            diagram.Objects.Add(diagramDimension);

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
