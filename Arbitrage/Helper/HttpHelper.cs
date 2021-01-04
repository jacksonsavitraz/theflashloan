using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Arbitrage.Helper
{
    public class HttpHelper : IDisposable
    {
        private const int TIMEOUT = 30;

        private readonly HttpClient _client;

        public HttpHelper(CookieContainer cookies = null)
        {
            var handler = new HttpClientHandler()
            {
                UseCookies = cookies != null,
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            if (cookies != null)
                handler.CookieContainer = cookies;

            _client = new HttpClient(handler, true)
            {
                Timeout = TimeSpan.FromSeconds(TIMEOUT),
            };

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36");
            _client.DefaultRequestHeaders.Add("Pragma", "no-cache");
            _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            _client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        }

        public string GetRequest(string url)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Add("Host", request.RequestUri.Host);
                    using (var requestTask = _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        requestTask.Wait();
                        using (var response = requestTask.Result)
                        {
                            using (var resultTask = response.Content.ReadAsStringAsync())
                            {
                                resultTask.Wait();
                                return resultTask.Result;
                            }
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public string PostJsonRequest(string url, object body)
        {
            try
            {
                using (var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"))
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content })
                    {
                        using (var requestTask = _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            requestTask.Wait();
                            using (var response = requestTask.Result)
                            {
                                using (var resultTask = response.Content.ReadAsStringAsync())
                                {
                                    resultTask.Wait();
                                    return resultTask.Result;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
