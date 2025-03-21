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

        public DiagramFrame(int width, int height, Color frameColour, float frameLineWeight, Color backgroundColour)
        {
            this.Width = width;
            this.Height = height;
            this.FrameColour = frameColour;
            this.FrameLineWeight = frameLineWeight;
            this.BackgroundColour = backgroundColour;
        }
    }
}