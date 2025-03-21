using DiagramLibrary.Core;
using DiagramLibrary.Text;

namespace DiagramLibrary
{
    public class DiagramTitle : IDiagramTitle
    {
        public IDiagramText TitleText { get; }

        public DiagramTitle(IDiagramText titleText)
        {
            this.TitleText = titleText;
        }
    }
}