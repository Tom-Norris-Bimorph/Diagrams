namespace DiagramLibrary.Core
{

    public interface IDiagramTitle
    {
        string TitleFont { get; }
        IDiagramText Title { get; }
    }
}