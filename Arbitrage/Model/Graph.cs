using System.Collections.Generic;

namespace Arbitrage.Model
{
    public class Graph
    {
        public Dictionary<Vertex, List<Edge>> VertexAdjacency { get; set; }
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }
    }
}
