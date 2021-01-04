using Arbitrage.Builder;

namespace Arbitrage
{
    public static class ArbitrageRunner
    {
        public static string Run()
        {
            var graph = GraphBuilder.Build();
            var cycles = CyclesBuilder.Build(graph);
            var opportunities = OpportunitiesBuilder.Build(graph, cycles);
            var output = OutputBuilder.Build(graph, opportunities);

            return output;
        }
    }
}
