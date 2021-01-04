namespace Arbitrage.Model
{
    public class TokenPair
    {
        public string Id { get; set; }

        public string BaseSymbol { get; set; }
        public string BaseContract { get; set; }
        public int BaseDecimals { get; set; }
        public double BasePrice { get; set; }

        public string QuoteSymbol { get; set; }
        public string QuoteContract { get; set; }
        public int QuoteDecimals { get; set; }
        public double QuotePrice { get; set; }
    }
}
