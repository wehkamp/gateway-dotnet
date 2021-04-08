using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProxyNet.Proxy;

namespace ProxyNet.Controllers
{
    [Controller]
    public class ProxyController : Controller
    {
        private readonly IProxyClient _proxyClient;

        private static Random _random = new Random();

        public ProxyController(IProxyClient proxyClient)
        {
            _proxyClient = proxyClient;
        }

        [HttpGet("")]
        public async Task Homepage()
        {
            var hostname = "content-site.trusted.nl.wehkamp.prod.blaze.ps";
            
            await _proxyClient.Proxy(HttpContext, hostname);
        }

        [HttpGet("/redirect")]
        public ActionResult Redirect()
        {
            return base.Redirect("http://example.org");
        }

        [HttpGet("/{**catch-all}")]
        public async Task Handle()
        {
            var hostname = "search-site.trusted.nl.wehkamp.prod.blaze.ps";
            
            await _proxyClient.Proxy(HttpContext, hostname);
        }
    }
}