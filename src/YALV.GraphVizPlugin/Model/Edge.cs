using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YALV.GraphVizPlugin.Model
{
    internal class Edge
    {
        private readonly List<int> logItems = new List<int>();
        public Edge(Node nodeA, Node nodeB, int logItemId, bool isDirected = true)
        {
            NodeA = nodeA;
            NodeB = nodeB;
            IsDirected = isDirected;
            NodeA.Register(this);
            if (!IsSelfTransition)
            {
                NodeB.Register(this);
            }

            logItems.Add(logItemId);
        }

        internal bool Connects(Node a, Node b)
        {
            if (NodeA == a && NodeB == b)
            {
                return true;
            }

            if (IsSelfTransition)
            {

            }

            return (!IsDirected || IsSelfTransition) && NodeA == b && NodeB == a;
        }

        public void AddItem(int id)
        {
            logItems.Add(id);
        }

        public bool IsSelfTransition
        {
            get
            {
                return NodeA == NodeB;
            }
        }

        public Node NodeA { get; private set; }
        public Node NodeB { get; private set; }
        public bool IsDirected { get; private set; }

        public override string ToString()
        {
            return $"{NodeA.Name} {(IsDirected ? "->" : "--")} {NodeB.Name} ({logItems.Count})";
        }
    }
}
