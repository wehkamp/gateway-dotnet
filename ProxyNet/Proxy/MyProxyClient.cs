using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Service.Proxy;

namespace ProxyNet.Proxy
{
    public class MyProxyClient : IProxyClient
    {
        private readonly HttpMessageInvoker _httpMessageInvoker = new(new SocketsHttpHandler()
        {
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false
        });

        private readonly MyHttpTransform _httpTransform = new();
        
        private readonly RequestProxyOptions _requestProxyOptions = new()
        {
            Timeout = TimeSpan.FromSeconds(100)
        };

        private readonly IHttpProxy _httpProxy;

        public MyProxyClient(IHttpProxy httpProxy)
        {
            _httpProxy = httpProxy;
        }

        public async Task Proxy(HttpContext httpContext, string hostname)
        {
            await _httpProxy.ProxyAsync(httpContext, $"https://{hostname}", _httpMessageInvoker, _requestProxyOptions, _httpTransform);
        }
    }
}