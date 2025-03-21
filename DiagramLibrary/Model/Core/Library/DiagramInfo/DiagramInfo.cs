using DiagramLibrary.Core;

namespace DiagramLibrary
{
    public class DiagramInfo : IDiagramInfo
    {
        public string LibraryVersion => typeof(Diagram).Assembly.GetName().Version.ToString();

        public IDiagramTitle Title { get; }

        public DiagramInfo(IDiagramTitle titleText)
        {
            this.Title = titleText;
        }
    }
}