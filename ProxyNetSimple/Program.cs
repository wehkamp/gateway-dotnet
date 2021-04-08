using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yarp.ReverseProxy.Service.Proxy;

namespace ProxyNetSimple
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureServices(s =>
                        {
                            s.AddHttpProxy();
                        })
                        .Configure(app =>
                        {
                            var httpProxy = app.ApplicationServices.GetService<IHttpProxy>();
                            
                            var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
                            {
                                UseProxy = false,
                                AllowAutoRedirect = false,
                                AutomaticDecompression = DecompressionMethods.None,
                                UseCookies = false
                            });

                            var transform = new CustomTransformer();
                            var requestOptions = new RequestProxyOptions();

                            app.UseRouting();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.Map("/{**catch-all}", async httpContext =>
                                {
                                    await httpProxy.ProxyAsync(
                                        httpContext, 
                                        "https://search-site.trusted.nl.wehkamp.prod.blaze.ps", 
                                        httpClient, 
                                        requestOptions, 
                                        transform
                                    );

                                    var errorFeature = httpContext.Features.Get<IProxyErrorFeature>();
                                    
                                    if (errorFeature != null)
                                    {
                                        var error = errorFeature.Error;
                                        var exception = errorFeature.Exception;
                                        
                                        // error handling
                                    }
                                });
                            });
                        });
                });
    }
    
    public class CustomTransformer : HttpTransformer
    {
        public override async Task TransformRequestAsync(HttpContext httpContext, HttpRequestMessage proxyRequest, string destinationPrefix)
        {
            await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix);
            proxyRequest.Headers.Host = null;
        }
    }
}