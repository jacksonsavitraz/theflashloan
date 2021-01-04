using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Arbitrage.Helper;
using Arbitrage.Model;
using Nethereum.Web3;

namespace Arbitrage.Service
{
    public static class UniswapService
    {
        private const string CURRENCY_API = "https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v2";

        public static TokenPair[] GetCurrencies()
        {
            var currencies = new List<TokenPair>();

            using (var http = new HttpHelper())
            {
                while (true)
                {
                    try
                    {
                        var json = http.PostJsonRequest(CURRENCY_API, new
                        {
                            operationName = "pairs",
                            variables = new { },
                            query = @"
query pairs {
    pairs (first: 500, orderBy: trackedReserveETH, orderDirection: desc) {
        id
    }
}"
                        });

                        var pairs = new HashSet<string>();
                        using (var document = JsonDocument.Parse(json, new JsonDocumentOptions() { AllowTrailingCommas = true }))
                        {
                            foreach (var element in document.RootElement.GetProperty("data").GetProperty("pairs").EnumerateArray())
                            {
                                var pair = element.GetProperty("id").GetString();
                                pairs.Add(pair);
                            }
                        }

                        json = http.PostJsonRequest(CURRENCY_API, new
                        {
                            operationName = "pairs",
                            variables = new { allPairs = pairs.ToArray() },
                            query = @"
fragment PairFields on Pair {
	id
	token0 {
		id
		symbol
		name
        decimals
	}
	token0Price
	token1 {
		id
		symbol
		name
        decimals
	}
	token1Price
}

query pairs($allPairs: [Bytes]!) {
	pairs (where: {id_in: $allPairs}, orderBy: trackedReserveETH, orderDirection: desc) {
		...PairFields
	}
}"
                        });

                        using (var document = JsonDocument.Parse(json, new JsonDocumentOptions() { AllowTrailingCommas = true }))
                        {
                            foreach (var element in document.RootElement.GetProperty("data").GetProperty("pairs").EnumerateArray())
                            {
                                var id = element.GetProperty("id").GetString();

                                var token0 = element.GetProperty("token0").GetProperty("symbol").GetString();
                                var token0Id = Web3.ToChecksumAddress(element.GetProperty("token0").GetProperty("id").GetString());
                                var token0Price = double.Parse(element.GetProperty("token0Price").GetString(), CultureInfo.InvariantCulture);
                                var token0Decimals = int.Parse(element.GetProperty("token0").GetProperty("decimals").GetString());

                                var token1 = element.GetProperty("token1").GetProperty("symbol").GetString();
                                var token1Id = Web3.ToChecksumAddress(element.GetProperty("token1").GetProperty("id").GetString());
                                var token1Price = double.Parse(element.GetProperty("token1Price").GetString(), CultureInfo.InvariantCulture);
                                var token1Decimals = int.Parse(element.GetProperty("token1").GetProperty("decimals").GetString());

                                if (token0.Equals("WETH", StringComparison.Ordinal))
                                    token0 = "ETH";

                                if (token1.Equals("WETH", StringComparison.Ordinal))
                                    token1 = "ETH";

                                currencies.Add(new TokenPair() {
                                    Id = id,

                                    BaseSymbol = token0,
                                    BasePrice = token0Price,
                                    BaseContract = token0Id,
                                    BaseDecimals = token0Decimals,

                                    QuoteSymbol = token1,
                                    QuotePrice = token1Price,
                                    QuoteContract = token1Id,
                                    QuoteDecimals = token1Decimals
                                });
                            }
                        }

                        break;
                    }
                    catch { }
                }
            }

            return currencies.ToArray();
        }
    }
}
