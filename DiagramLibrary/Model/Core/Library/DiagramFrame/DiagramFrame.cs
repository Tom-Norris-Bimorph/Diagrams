using DiagramLibrary.Core;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramFrame : IDiagramFrame
    {
        public int Width { get; }
        public int Height { get; }

        public Color FrameColour { get; }
        public float FrameLineWeight { get; }

        public Color BackgroundColour { get; }
    }
}