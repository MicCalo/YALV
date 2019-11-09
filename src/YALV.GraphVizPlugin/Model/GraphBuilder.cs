using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YALV.GraphVizPlugin.Model
{
    internal class GraphBuilder
    {
        private readonly Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        public void AddTransition(string from, string to, int logItemId)
        {
            Node a, b;
            if (!nodes.TryGetValue(from, out a))
            {
                a = new Node(from);
                nodes.Add(from, a);
            }
            if (!nodes.TryGetValue(to, out b))
            {
                b = new Node(to);
                nodes.Add(to, b);
            }

            Edge e = a.FindEdge(b);
            if (e == null)
            {
                e = new Edge(a, b, logItemId);
            }
            else
            {
                e.AddItem(logItemId);
            }
        }

        public IReadOnlyList<Edge> Edges
        {
            get
            {
                List<Edge> addedEdges = new List<Edge>();
                foreach (Node node in nodes.Values)
                {
                    foreach (Edge edge in node.Edges)
                    {
                        if (!addedEdges.Contains(edge))
                        {
                            addedEdges.Add(edge);
                        }
                    }
                }
                return addedEdges;
            }
        }

        public Graph CreateGraph()
        {
            Graph graph = new Graph("graph");

            foreach (Edge e in Edges)
            {
                graph.AddEdge(e.NodeA.Name, e.NodeB.Name);
            }

            return graph;
        }

        public GViewer CreateViewer()
        {
            GViewer result = new GViewer();

            result.Graph = CreateGraph();
            return result;
        }
    }
}
