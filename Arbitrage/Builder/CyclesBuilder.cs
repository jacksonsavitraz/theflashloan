using System;
using System.Collections.Generic;
using Arbitrage.Helper;
using Arbitrage.Model;

namespace Arbitrage.Builder
{
    public static class CyclesBuilder
    {
        public static List<List<Vertex>> Build(Graph graph)
        {
            var cycles = new List<List<Vertex>>();
            var distances = new Dictionary<Vertex, double>();
            var predecessor = new Dictionary<Vertex, Vertex>();

            foreach (var vertex in graph.Vertices)
            {
                var vertexCycles = GetVertexCycles(vertex, graph, distances, predecessor);
                cycles.AddRange(vertexCycles);
            }

            var uniqueCycles = GetUniqueCycles(cycles);
            return uniqueCycles;
        }

        private static List<List<Vertex>> GetVertexCycles(Vertex source, Graph graph, Dictionary<Vertex, double> distances, Dictionary<Vertex, Vertex> predecessor)
        {
            foreach (var vertex in graph.Vertices)
            {
                distances[vertex] = double.PositiveInfinity;
                predecessor[vertex] = vertex;
            }

            distances[source] = 0;

            RelaxEdges(graph, distances, predecessor);

            var negativeWeightCycles = GetNegativeWightCycles(graph, distances, predecessor);
            return negativeWeightCycles;
        }

        private static List<List<Vertex>> GetNegativeWightCycles(Graph graph, Dictionary<Vertex, double> distances, Dictionary<Vertex, Vertex> predecessor)
        {
            var cycles = new List<List<Vertex>>();
            var visitedVertices = new HashSet<Vertex>();

            foreach (var edge in graph.Edges)
            {
                if (visitedVertices.Contains(edge.End))
                    continue;

                var weight = NormalizeWeight(edge.Weight);
                if (distances.ContainsKey(edge.Start) && distances.ContainsKey(edge.End) && distances[edge.End] > distances[edge.Start] + weight)
                {
                    var newCycle = new List<Vertex>();
                    var vertex = edge.End;

                    do
                    {
                        visitedVertices.Add(vertex);
                        newCycle.Add(vertex);
                        vertex = predecessor[vertex];
                    } while (vertex != edge.End && !newCycle.Contains(vertex));

                    var index = newCycle.IndexOf(vertex);
                    newCycle.Add(vertex);
                    cycles.Add(newCycle.GetRange(index, newCycle.Count - index));
                }
            }

            return cycles;
        }

        private static void RelaxEdges(Graph graph, Dictionary<Vertex, double> distances, Dictionary<Vertex, Vertex> predecessor)
        {
            for (var i=0; i<distances.Count - 1; i++)
            {
                foreach (var edge in graph.Edges)
                {
                    var weight = NormalizeWeight(edge.Weight);
                    if (distances.ContainsKey(edge.Start) && distances.ContainsKey(edge.End) && distances[edge.End] > distances[edge.Start] + weight)
                    {
                        distances[edge.End] = distances[edge.Start] + weight;
                        predecessor[edge.End] = edge.Start;
                    }
                }
            }
        }

        private static double NormalizeWeight(double weight)
        {
            return -Math.Log(weight);
        }

        private static List<List<Vertex>> GetUniqueCycles(List<List<Vertex>> cycles)
        {
            var uniqueCycles = new HashSet<List<Vertex>>(cycles, new SequenceHelper<Vertex>());
            return new List<List<Vertex>>(uniqueCycles);
        }
    }
}
