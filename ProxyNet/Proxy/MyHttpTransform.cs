using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Service.Proxy;

namespace ProxyNet.Proxy
{
    public class MyHttpTransform : HttpTransformer
    {
        public override async Task TransformRequestAsync(HttpContext httpContext,
            HttpRequestMessage proxyRequest, string destinationPrefix)
        {
            await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix);
            proxyRequest.Headers.Host = null;
        }
    }
}