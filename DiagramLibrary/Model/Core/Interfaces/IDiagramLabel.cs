using DiagramLibrary.Core;
using DiagramLibrary.Text;

namespace DiagramLibrary
{
    public interface IDiagramLabel : IDrawableDiagramObject
    {
        IDiagramLabelLeader Leader { get; }
        IDiagramText DiagramText { get; }
    }
}