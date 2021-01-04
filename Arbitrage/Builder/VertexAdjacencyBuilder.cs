using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Arbitrage.Model;
using Arbitrage.Service;

namespace Arbitrage.Builder
{
    public static class VertexAdjacencyBuilder
    {
        public static Dictionary<Vertex, List<Edge>> Build()
        {
            var vertices = new Dictionary<string, Vertex>();
            var adjacencyVertex = new Dictionary<Vertex, List<Edge>>();

            foreach (var currency in UniswapService.GetCurrencies())
            {
                if (!vertices.ContainsKey(currency.BaseSymbol))
                    vertices.Add(currency.BaseSymbol, new Vertex() { Symbol = currency.BaseSymbol, Contract = currency.BaseContract, Decimals = currency.BaseDecimals });

                if (!vertices.ContainsKey(currency.QuoteSymbol))
                    vertices.Add(currency.QuoteSymbol, new Vertex() { Symbol = currency.QuoteSymbol, Contract = currency.QuoteContract, Decimals = currency.QuoteDecimals });

                var start = vertices[currency.BaseSymbol];
                var end = vertices[currency.QuoteSymbol];

                if (!adjacencyVertex.ContainsKey(start))
                    adjacencyVertex.Add(start, new List<Edge>());

                adjacencyVertex[start].Add(new Edge() { Start = start, End = end, Weight = currency.QuotePrice });

                if (!adjacencyVertex.ContainsKey(end))
                    adjacencyVertex.Add(end, new List<Edge>());

                adjacencyVertex[end].Add(new Edge() { Start = end, End = start, Weight = currency.BasePrice });
            }

            return adjacencyVertex;
        }
    }
}
