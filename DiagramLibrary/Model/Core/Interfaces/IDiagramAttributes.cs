using System.Drawing;

namespace DiagramLibrary.Core
{
    public interface IDiagramAttributes
    {
        Color Colour { get; }
        float LineWeight { get; }
    }
}