using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProxyNet.Proxy
{
    public interface IProxyClient
    {
        Task Proxy(HttpContext httpContext, string hostname);
    }
}