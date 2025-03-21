using DiagramLibrary.Core;
using System.Collections.Generic;

namespace DiagramLibrary
{
    public interface IDiagramObjectConverter
    {
        IDiagramObjectSet Convert(Grasshopper.Kernel.GH_Component component,
            List<object> objects, double tolerance);

        IDiagramObjectSet AddDiagramObjectFromGoo(
            Grasshopper.Kernel.GH_Component component, object obj);

        IDiagramObjectSet AddRhinoObject(Grasshopper.Kernel.GH_Component component,
            object obj, double tolerance);
    }
}