using System.Collections.Generic;

namespace DiagramLibrary.Core
{

    public interface IDiagramObjectSet : IEnumerable<IDiagramObject>
    {
        int Count { get; }

        void Add(IDiagramObject obj);

        void AddRange(IEnumerable<IDiagramObject> diagramObjects);
    }
}