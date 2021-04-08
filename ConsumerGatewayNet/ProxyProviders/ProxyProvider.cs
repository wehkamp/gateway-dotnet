using System.Linq;
using System.Threading;
using ConsumerGatewayNet.Consul;
using ConsumerGatewayNet.RoutingArbitrage;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Service;

namespace ConsumerGatewayNet.ProxyProviders
{
    public class ProxyProvider : IProxyConfigProvider
    {
        private RoutingArbitrageProxyProvider _routingArbitrageProxyProvider;
        private ConsulProxyProvider _consulProxyProvider;
        
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public ProxyProvider(RoutingArbitrageProxyProvider routingArbitrageProxyProvider, ConsulProxyProvider consulProxyProvider)
        {
            _routingArbitrageProxyProvider = routingArbitrageProxyProvider;
            _consulProxyProvider = consulProxyProvider;
        }

        public IProxyConfig GetConfig()
        {
            var routingArbitrageRoutes = _routingArbitrageProxyProvider.GetRoutes();
            var consulRoutes = _consulProxyProvider.GetRoutes();

            var allRoutes = routingArbitrageRoutes.Concat(consulRoutes).ToList();

            var routingArbitrageClusters = _routingArbitrageProxyProvider.GetClusters();
            var consulClusters = _consulProxyProvider.GetClusters();

            var allClusters = routingArbitrageClusters.Concat(consulClusters).ToList();
            
            return new ProxyConfig
            {
                Routes = allRoutes,
                Clusters = allClusters,
                ChangeToken = new CancellationChangeToken(_cts.Token)
            };
        }
    }
}