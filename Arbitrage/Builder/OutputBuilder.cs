using System.Text;
using System.Linq;
using System.Collections.Generic;
using Arbitrage.Model;

namespace Arbitrage.Builder
{
    public static class OutputBuilder
    {
        public static string Build(Graph graph, Dictionary<List<Vertex>, double> opportunities)
        {
            var found = new List<KeyValuePair<string, double>>();
            foreach (var opportunity in opportunities.Keys)
            {
                var sb = new StringBuilder();
                var balance = 1D;

                for (var i=1; i<opportunity.Count; i++)
                {
                    var token0 = opportunity[i-1];
                    var token1 = opportunity[i];

                    if (i==1)
                        sb.AppendFormat("{0:0.00000000} {1}", balance, token0.Symbol);

                    Edge edge = null;
                    foreach (var adj in graph.VertexAdjacency[token0])
                    {
                        if (adj.End == token1)
                        {
                            edge = adj;
                            break;
                        }
                    }

                    if (edge != null)
                    {
                        balance = (balance - (balance * 0.003)) * edge.Weight;
                    }

                    sb.AppendFormat(" → {0:0.00000000} {1}", balance, token1.Symbol);
                }

                var perc = (((balance / 1) - 1) * 100) - 0.0009;
                sb.AppendFormat(" ≈ {0:0.000}% ", perc);

                if (perc > 0)
                {
                    var amount = "1000000";
                    sb.AppendLine();
                    sb.AppendLine(amount.PadRight(amount.Length + opportunity[0].Decimals, '0'));
                    sb.Append('[');
                    for (var i = 0; i < opportunity.Count; i++)
                    {
                        if (i != 0)
                            sb.Append(',');
                        sb.AppendFormat("\"{0}\"", opportunity[i].Contract);
                    }
                    sb.Append(']');
                    sb.AppendLine();
                    sb.AppendLine();

                    found.Add(new KeyValuePair<string, double>(sb.ToString(), perc));
                }
            }

            if (found.Count > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Oportuniades encontradas:");
                sb.AppendLine();
                foreach (var item in found.OrderByDescending(x => x.Value))
                    sb.AppendLine(item.Key);
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
