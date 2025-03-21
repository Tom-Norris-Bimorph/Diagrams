using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace DiagramLibrary
{

    public class DiagramObjectConverter : IDiagramObjectConverter
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
                case Rhino.DocObjects.ObjectType.Curve:
                    goo.CastTo(out Curve crv);

                    var diagramCurve =
                        new DiagramCurve(crv, DiagramDefaults.DefaultDiagramAttributes);

                    diagramObjects.Add(diagramCurve);
                    break;
                case Rhino.DocObjects.ObjectType.Surface:
                    {
                        goo.CastTo(out Surface srf);

                        var brepCurves = this.AddBrep(srf.ToBrep());

                        diagramObjects.AddRange(brepCurves);
                    }
                    break;
                case Rhino.DocObjects.ObjectType.Brep:
                    {
                        goo.CastTo(out Brep brep);
                        var brepCurves = this.AddBrep(brep);
                        diagramObjects.AddRange(brepCurves);
                    }

                    break;
                case Rhino.DocObjects.ObjectType.Extrusion:
                    {
                        goo.CastTo(out Extrusion ext);

                        var brepCurves = this.AddBrep(ext.ToBrep());

                        diagramObjects.AddRange(brepCurves);
                    }

                    break;
                case Rhino.DocObjects.ObjectType.None:
                case Rhino.DocObjects.ObjectType.Point:
                case Rhino.DocObjects.ObjectType.PointSet:
                case Rhino.DocObjects.ObjectType.Mesh:
                case Rhino.DocObjects.ObjectType.Light:
                case Rhino.DocObjects.ObjectType.Annotation:
                case Rhino.DocObjects.ObjectType.InstanceDefinition:
                case Rhino.DocObjects.ObjectType.InstanceReference:
                case Rhino.DocObjects.ObjectType.TextDot:
                case Rhino.DocObjects.ObjectType.Grip:
                case Rhino.DocObjects.ObjectType.Detail:
                case Rhino.DocObjects.ObjectType.Hatch:
                case Rhino.DocObjects.ObjectType.MorphControl:
                case Rhino.DocObjects.ObjectType.SubD:
                case Rhino.DocObjects.ObjectType.BrepLoop:
                case Rhino.DocObjects.ObjectType.PolysrfFilter:
                case Rhino.DocObjects.ObjectType.EdgeFilter:
                case Rhino.DocObjects.ObjectType.PolyedgeFilter:
                case Rhino.DocObjects.ObjectType.MeshVertex:
                case Rhino.DocObjects.ObjectType.MeshEdge:
                case Rhino.DocObjects.ObjectType.MeshFace:
                case Rhino.DocObjects.ObjectType.Cage:
                case Rhino.DocObjects.ObjectType.Phantom:
                case Rhino.DocObjects.ObjectType.ClipPlane:
                case Rhino.DocObjects.ObjectType.AnyObject:
                default:
                    break;
            }

            return diagramObjects;
        }

        private IDiagramObjectSet AddBrep(Brep brep)
        {
            var diagramObjects = new DiagramObjectSet();

            var curves = DiagramHatch.CreateFromBrep(brep, DiagramDefaults.DefaultDiagramAttributes, DiagramDefaults.DefaultHatchAttributes);

            diagramObjects.AddRange(curves);

            return diagramObjects;
        }
    }
}