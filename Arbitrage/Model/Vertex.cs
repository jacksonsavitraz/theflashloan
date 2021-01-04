using System;

namespace Arbitrage.Model
{
    public class Vertex : IEquatable<Vertex>
    {
        public string Symbol { get; set; }
        public int Decimals { get; set; }
        public string Contract { get; set; }

        public bool Equals(Vertex other)
        {
            return other != null && Symbol.Equals(other.Symbol, StringComparison.Ordinal);
        }
    }
}
