using DiagramLibrary.Core;
using DiagramLibrary.Defaults;
using Rhino.Geometry;

namespace DiagramLibrary
{
    public class DiagramTitle : IDiagramTitle
    {

        public string TitleFont { get; } = DiagramDefaults.DefaultFontName;

        public IDiagramText TitleText { get; }

        public DiagramTitle(string title, PointF location, Color colour, float textSize, TextJustification justification, Color maskColour, Color frameColor, float frameLineWeight, string fontName, SizeF size, float padding, TextJustification textJustification)
        {
            this.TitleText = new DiagramText(title, location, colour, textSize, justification, maskColour, frameColor, frameLineWeight, fontName, size, padding, textJustification);
        }
    }
}