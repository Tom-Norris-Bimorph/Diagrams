namespace DiagramLibrary.Core
{
    public interface IDiagramInfo
    {
        string LibraryVersion { get; }
        IDiagramTitle Title { get; }
    }
}