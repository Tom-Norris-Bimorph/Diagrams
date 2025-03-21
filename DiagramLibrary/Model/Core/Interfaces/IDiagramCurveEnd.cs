using DiagramLibrary.Core;

namespace DiagramLibrary
{
    public interface IDiagramCurveEnd : IDrawableDiagramObject
    {
        void Flip();

        IDiagramCurveEnd DuplicateCurveEnd();
    }
}