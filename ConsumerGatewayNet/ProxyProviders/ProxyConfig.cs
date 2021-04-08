using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Abstractions;
using Yarp.ReverseProxy.Service;

namespace ConsumerGatewayNet.ProxyProviders
{
    public class ProxyConfig : IProxyConfig
    {
        public IReadOnlyList<ProxyRoute> Routes { get; set; }
        public IReadOnlyList<Cluster> Clusters { get; set; }
        
        public IChangeToken ChangeToken { get; set; }
    }
}