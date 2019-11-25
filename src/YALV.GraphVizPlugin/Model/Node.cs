using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YALV.GraphVizPlugin.Model
{
    internal class Node
    {
        private readonly List<Edge> edges = new List<Edge>();

        public Node(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public void Register(Edge e)
        {
            edges.Add(e);
        }

        internal Edge FindEdge(Node b)
        {
            return edges.Find(x => x.Connects(this, b));
        }

        public IReadOnlyList<Edge> Edges { get { return edges.AsReadOnly(); } }

        public override string ToString()
        {
            return $"{Name} ({edges.Count})";
        }
    }
}
