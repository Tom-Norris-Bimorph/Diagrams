using DiagramLibrary.Core;
using System.Collections.Generic;

namespace DiagramLibrary
{
    public interface IDiagramHatch : IBaseCurveDiagramObject
    {
        Rhino.Display.DisplayMaterial CachedMaterial { get; }
        Rhino.Display.DisplayMaterial CachedSelectedMaterial { get; }
        IList<IDiagramCurve> InnerCurves { get; }
        IList<IDiagramCurve> OuterCurves { get; }
        IDiagramHatchAttributes DiagramHatchAttributes { get; }
        IDiagramCurveAttributes DiagramCurveAttributes { get; }
    }
}