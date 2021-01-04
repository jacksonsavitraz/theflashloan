using System.Collections.Generic;
using Arbitrage.Model;

namespace Arbitrage.Builder
{
    public static class GraphBuilder
    {
        public static Graph Build()
        {
            var edges = new List<Edge>();
            var vertices = new List<Vertex>();
            var vertexAdjacency = VertexAdjacencyBuilder.Build();

            foreach (var vertex in vertexAdjacency)
                edges.AddRange(vertex.Value);

            vertices.AddRange(vertexAdjacency.Keys);

            return new Graph() { VertexAdjacency = vertexAdjacency, Vertices = vertices, Edges = edges };
        }
    }
}
