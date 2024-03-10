using DiagramLibrary;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiagramsForGrasshopper
{
    public abstract class DiagramComponent : GH_Component, IGH_BakeAwareObject

    {

                      

        protected bool m_VersionChecked = false;
        protected Transform m_Xform = Transform.ZeroTransformation;
   

        public DiagramComponent(string a, string b, string c, string d, string e)
           : base(a, b, c, d, e) // pass through the GH_Comp
        {
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        //base.RegisterOutputParams(pManager);
        pManager.AddTextParameter("Report", "R", "A report from this componant", GH_ParamAccess.list);
        pManager.AddGenericParameter("Diagram Objects", "DObjs", "The Diagram Objects Created by this componant", GH_ParamAccess.list);

        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.ClearRuntimeMessages();
            m_Xform = Transform.ZeroTransformation;
            this.CheckLibraryVersion(DA);

            Diagram diagram = DiagramSolveInstance(DA);



            DA.SetData(1, diagram);

            DA.SetDataList(0, this.RuntimeMessages(GH_RuntimeMessageLevel.Blank | GH_RuntimeMessageLevel.Error | GH_RuntimeMessageLevel.Remark | GH_RuntimeMessageLevel.Warning));
        }

    
        public abstract DiagramLibrary.Diagram DiagramSolveInstance(IGH_DataAccess DA);



        public override BoundingBox ClippingBox
        {

            get
            {
                BoundingBox returnBox = BoundingBox.Empty;
                try
                {


                    var diagramOutput = this.Params.Output[1].VolatileData;
                    for (int i = 0; i < diagramOutput.PathCount; i++)
                    {
                        var diagramGooList = diagramOutput.get_Branch(diagramOutput.Paths[i]);

                        for (int j = 0; j < diagramGooList.Count; j++)
                        {
                            var diagramGoo = diagramGooList[j] as Grasshopper.Kernel.Types.IGH_Goo;
                            diagramGoo.CastTo(out Diagram diagram);

                            if (diagram == null) { continue; }
                            returnBox.Union(diagram.GetGeometryBoundingBox());


                        }

                    }


                }
                catch (Exception)
                {


                }

                return returnBox;

            }
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            try
            {

                if (!this.Locked)
                {
                    var diagramOutput = this.Params.Output[1].VolatileData;
                    for (int i = 0; i < diagramOutput.PathCount; i++)
                    {
                        var diagramGooList = diagramOutput.get_Branch(diagramOutput.Paths[i]);

                        for (int j = 0; j < diagramGooList.Count; j++)
                        {
                            var diagramGoo = diagramGooList[j] as Grasshopper.Kernel.Types.IGH_Goo;
                            diagramGoo.CastTo(out Diagram diagram);

                            if (diagram == null) { continue; }

                            DrawState state = DrawState.Normal;
                            if (this.m_attributes.Selected) {
                                state = DrawState.Selected;
                            }
                            diagram.DrawRhinoPreview(args.Display, GH_Component.DocumentTolerance(), m_Xform, state);
                        }
                    }

                }


            }
            catch (Exception)
            {


            }


        }


        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
           Menu_AppendItem(menu, "Bake", BakeHandler, true, false);
            
           // var setScale = Menu_AppendItem(menu, "Set Number");
            //Menu_AppendTextItem(setScale.DropDown, Scale.ToString(System.Globalization.CultureInfo.InvariantCulture), null, TextChanged, true);
        }

        private void BakeHandler(object sender, EventArgs e)
        {
            var diagramOutput = this.Params.Output[1].VolatileData;
            Rhino.RhinoDoc doc = Rhino.RhinoDoc.ActiveDoc;
            for (int i = 0; i < diagramOutput.PathCount; i++)
            {
                var diagramGooList = diagramOutput.get_Branch(diagramOutput.Paths[i]);

                for (int j = 0; j < diagramGooList.Count; j++)
                {
                    var diagramGoo = diagramGooList[j] as Grasshopper.Kernel.Types.IGH_Goo;
                    diagramGoo.CastTo(out Diagram diagram);

                    if (diagram == null) { continue; }

                    DrawState state = DrawState.Normal;
                

                    diagram.BakeRhinoPreview(GH_Component.DocumentTolerance(), m_Xform, state,doc,doc.CreateDefaultAttributes(),out List<Guid> guids);
                }
            }
        }

        private void CheckLibraryVersion(IGH_DataAccess DA)
        {
            if (m_VersionChecked == false)
            {
                string libraryVersion = DiagramLibrary.Diagram.LibraryVersion();
                string componantLibraryVersion = "1.0.0.1";
                VersionComparision compareVersion = CompareVersion(libraryVersion, componantLibraryVersion);
                switch (compareVersion)
                {

                    case VersionComparision.VersionMatch:

                        m_VersionChecked = true;

                        break;

                    case VersionComparision.LibraryVersionIsNewer:
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,"ERROR01: The Diagrams Library (.dll) currently loaded in grasshopper is newer than the library this component was built against therefore results might be unpredicatble, error prone and newer features will be ignored. Check for an update on Food for Rhino or the Package Manager. Library Version: " + libraryVersion + ", Componant was Built Against Version:" + componantLibraryVersion);
                        break;
                    case VersionComparision.ComponantVersionIsNewer:
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,"ERROR02: This Component was built against a newer version of the Diagrams Library (.dll) than the one currently loaded in grasshopper therefore results might be unpredicatble, error prone and newer features will be ignored. Check for an update on Food for Rhino or the Package Manager for any packages that create diagrams. Library Version: " + libraryVersion + ", Componant was Built Against Version:" + componantLibraryVersion);
                        break;
                }




            }
        }

        private VersionComparision CompareVersion(string libraryVersion, string componantVersion)
        {
            string[] libraryVersionSplit = libraryVersion.Split('.');
            string[] componantVersionSplit = componantVersion.Split('.');

            for (int i = 0; i < libraryVersionSplit.Length; i++)
            {
                if (i > componantVersionSplit.Length)
                {
                    return VersionComparision.LibraryVersionIsNewer;
                }
                int libraryInt = Int32.Parse(libraryVersionSplit[i]);
                int componantInt = Int32.Parse(componantVersionSplit[i]);

                if (libraryInt > componantInt)
                {
                    return VersionComparision.LibraryVersionIsNewer;
                }

                if (libraryInt < componantInt)
                {
                    return VersionComparision.ComponantVersionIsNewer;
                }

            }

            return VersionComparision.VersionMatch;

        }


        private enum VersionComparision
        {
            VersionMatch,
            LibraryVersionIsNewer,
            ComponantVersionIsNewer
        }




        public override void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            obj_ids = new List<Guid>();

            var diagramOutput = this.Params.Output[1].VolatileData;
          
            for (int i = 0; i < diagramOutput.PathCount; i++)
            {
                var diagramGooList = diagramOutput.get_Branch(diagramOutput.Paths[i]);

                for (int j = 0; j < diagramGooList.Count; j++)
                {
                    var diagramGoo = diagramGooList[j] as Grasshopper.Kernel.Types.IGH_Goo;
                    diagramGoo.CastTo(out Diagram diagram);

                    if (diagram == null) { continue; }

                    DrawState state = DrawState.Normal;


                    diagram.BakeRhinoPreview(GH_Component.DocumentTolerance(), m_Xform, state, doc, doc.CreateDefaultAttributes(), out List<Guid> guids);
                    obj_ids.AddRange(guids);
                    
                }
            }



         
        }

        public override void BakeGeometry(RhinoDoc doc, Rhino.DocObjects.ObjectAttributes att, List<Guid> obj_ids)
        {
            obj_ids = new List<Guid>();

            var diagramOutput = this.Params.Output[1].VolatileData;

            for (int i = 0; i < diagramOutput.PathCount; i++)
            {
                var diagramGooList = diagramOutput.get_Branch(diagramOutput.Paths[i]);

                for (int j = 0; j < diagramGooList.Count; j++)
                {
                    var diagramGoo = diagramGooList[j] as Grasshopper.Kernel.Types.IGH_Goo;
                    diagramGoo.CastTo(out Diagram diagram);

                    if (diagram == null) { continue; }

                    DrawState state = DrawState.Normal;


                    diagram.BakeRhinoPreview(GH_Component.DocumentTolerance(), m_Xform, state, doc, att, out List<Guid> guids);
                    obj_ids.AddRange(guids);

                }
            }


        }

        public override bool IsBakeCapable => true;






    }

}
