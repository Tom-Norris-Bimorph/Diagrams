using DiagramLibrary.Core;
using System.Collections.Generic;

namespace DiagramLibrary
{

    public class DiagramObjectSet : IDiagramObjectSet
    {
        private readonly IList<IDiagramObject> _objects = new List<IDiagramObject>();

        public int Count => _objects.Count;

        public void Add(IDiagramObject obj)
        {
            _objects.Add(obj);
        }

        public void AddRange(IEnumerable<IDiagramObject> diagramObjects)
        {
            foreach (var diagramObject in diagramObjects)
            {
                this.Add(diagramObject);
            }
        }

        public IEnumerator<IDiagramObject> GetEnumerator() => _objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    }
}