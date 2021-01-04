using System;
using System.Collections.Generic;
using Arbitrage.Helper;
using Arbitrage.Model;

namespace Arbitrage.Builder
{
    public static class OpportunitiesBuilder
    {
        public static Dictionary<List<Vertex>, double> Build(Graph graph, List<List<Vertex>> cycles)
        {
            var opportunities = new Dictionary<List<Vertex>, double>(new SequenceHelper<Vertex>());

            foreach (var cycle in cycles)
            {
                var totalWeight = 0D;
                cycle.Reverse();

                for (var i=1; i<cycle.Count; i++)
                {
                    var u = cycle[i - 1];
                    var v = cycle[i];
                    var adj = graph.VertexAdjacency[u];
                    foreach (var edge in adj)
                    {
                        if (edge.Start == u && edge.End == v)
                        {
                            totalWeight += NormalizeWeight(edge.Weight);
                            break;
                        }
                    }
                }

                totalWeight = DenormalizeWeight(totalWeight);
                opportunities[cycle] = totalWeight;
            }

            return opportunities;
        }

        private static double NormalizeWeight(double weight)
        {
            return -Math.Log(weight);
        }

        private static double DenormalizeWeight(double weight)
        {
            return Math.Pow(2, -weight) -1;
        }
    }
}
