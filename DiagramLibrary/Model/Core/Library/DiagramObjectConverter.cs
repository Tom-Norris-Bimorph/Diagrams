using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace DiagramLibrary
{

    public class DiagramObjectConverter
    {

        public IDiagramObjectSet Convert(Grasshopper.Kernel.GH_Component component,
            List<object> objects, double tolerance)
        {
            var diagramObjects = new DiagramObjectSet();

            for (var i = 0; i < objects.Count; i++)
            {
                try
                {
                    var goo = objects[i] as Grasshopper.Kernel.Types.IGH_GeometricGoo;

                    goo.CastTo(out GeometryBase geoBase);

                    var rhinoObjectConverted =
                        this.AddRhinoObject(component, objects[i], tolerance);

                    diagramObjects.AddRange(rhinoObjectConverted);
                }
                catch (Exception)
                {
                    var gooObjectConverted = this.AddDiagramObjectFromGoo(component, objects[i]);

                    diagramObjects.AddRange(gooObjectConverted);
                }
            }

            return diagramObjects;
        }

        public IDiagramObjectSet AddDiagramObjectFromGoo(
            Grasshopper.Kernel.GH_Component component, object obj)
        {
            var diagramObjects = new DiagramObjectSet();

            if (obj == null)
            {
                component.AddRuntimeMessage(Grasshopper.Kernel.GH_RuntimeMessageLevel.Warning,
                    "Cannot add null object to diagram");
                return diagramObjects;
            }

            var goo = obj as Grasshopper.Kernel.Types.IGH_Goo;

            goo.CastTo(out Diagram diagram);

            if (diagram == null || diagram.Objects.Count == 0)
            {
                return diagramObjects;
            }

            diagramObjects.AddRange(diagram.Objects);

            return diagramObjects;
        }

        public IDiagramObjectSet AddRhinoObject(Grasshopper.Kernel.GH_Component component,
            object obj, double tolerance)
        {
            var diagramObjects = new DiagramObjectSet();

            if (obj == null)
            {
                return diagramObjects;
            }

            var goo = obj as Grasshopper.Kernel.Types.IGH_GeometricGoo;
            goo.CastTo(out GeometryBase geoBase);

            switch (geoBase.ObjectType)
            {
                case Rhino.DocObjects.ObjectType.None:
                    break;
                case Rhino.DocObjects.ObjectType.Point:
                    break;
                case Rhino.DocObjects.ObjectType.PointSet:
                    break;
                case Rhino.DocObjects.ObjectType.Curve:
                    goo.CastTo(out Curve crv);

                    var diagramCurve =
                        new DiagramCurve(crv, DiagramDefaults.DefaultDiagramAttributes);
                    diagramObjects.Add(diagramCurve);
                    break;
                case Rhino.DocObjects.ObjectType.Surface:
                    goo.CastTo(out Surface srf);

                    var brepCurves = this.AddBrep(tolerance, srf.ToBrep());
                    diagramObjects.AddRange(brepCurves);
                    break;
                case Rhino.DocObjects.ObjectType.Brep:

                    goo.CastTo(out Brep brep);
                    var brepCurves = this.AddBrep(tolerance, brep);
                    diagramObjects.AddRange(brepCurves);
                    break;
                case Rhino.DocObjects.ObjectType.Mesh:
                    break;
                case Rhino.DocObjects.ObjectType.Light:
                    break;
                case Rhino.DocObjects.ObjectType.Annotation:
                    break;
                case Rhino.DocObjects.ObjectType.InstanceDefinition:
                    break;
                case Rhino.DocObjects.ObjectType.InstanceReference:
                    break;
                case Rhino.DocObjects.ObjectType.TextDot:
                    break;
                case Rhino.DocObjects.ObjectType.Grip:
                    break;
                case Rhino.DocObjects.ObjectType.Detail:
                    break;
                case Rhino.DocObjects.ObjectType.Hatch:

                    break;
                case Rhino.DocObjects.ObjectType.MorphControl:
                    break;
                case Rhino.DocObjects.ObjectType.SubD:
                    break;
                case Rhino.DocObjects.ObjectType.BrepLoop:
                    break;
                case Rhino.DocObjects.ObjectType.PolysrfFilter:
                    break;
                case Rhino.DocObjects.ObjectType.EdgeFilter:
                    break;
                case Rhino.DocObjects.ObjectType.PolyedgeFilter:
                    break;
                case Rhino.DocObjects.ObjectType.MeshVertex:
                    break;
                case Rhino.DocObjects.ObjectType.MeshEdge:
                    break;
                case Rhino.DocObjects.ObjectType.MeshFace:
                    break;
                case Rhino.DocObjects.ObjectType.Cage:
                    break;
                case Rhino.DocObjects.ObjectType.Phantom:
                    break;
                case Rhino.DocObjects.ObjectType.ClipPlane:
                    break;
                case Rhino.DocObjects.ObjectType.Extrusion:
                    goo.CastTo(out Extrusion ext);

                    var brepCurves = this.AddBrep(tolerance, ext.ToBrep());
                    diagramObjects.AddRange(brepCurves);
                    break;
                case Rhino.DocObjects.ObjectType.AnyObject:
                    break;
                default:
                    break;
            }

            return diagramObjects;
        }

        private IDiagramObjectSet AddBrep(double tolernace, Brep brep)
        {
            var diagramObjects = new DiagramObjectSet();

            var curves = DiagramFilledCurve.CreateFromBrep(brep,
                DiagramDefaults.DefaultColor, DiagramDefaults.DefaultColor,
                DiagramDefaults.DefaultLineWeight);

            diagramObjects.AddRange(curves);

            return diagramObjects;
        }
    }
}