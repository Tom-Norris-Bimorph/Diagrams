using System.Drawing;

namespace DiagramLibrary.Core
{
    public interface IDiagramFrame
    {
        int Width { get; }
        int Height { get; }
        Color FrameColour { get; }
        float FrameLineWeight { get; }
        Color BackgroundColour { get; }
    }
}