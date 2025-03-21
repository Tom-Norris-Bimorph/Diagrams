using Rhino.Geometry;

namespace DiagramLibrary.Text
{
    public interface IDiagramTextJustification
    {
        TextJustification Justification { get; }
        bool IsCenter { get; }

        bool IsRight { get; }

        bool IsTop { get; }

        bool IsMiddle { get; }
    }
}