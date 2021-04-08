using System.Collections.Generic;
using Yarp.ReverseProxy.Abstractions;

namespace ConsumerGatewayNet.RoutingArbitrage
{
    public class RoutingArbitrageProxyProvider
    {
        private RoutingArbitrageClient _routingArbitrageClient;

        public RoutingArbitrageProxyProvider(RoutingArbitrageClient routingArbitrageClient)
        {
            _routingArbitrageClient = routingArbitrageClient;
        }

        public List<ProxyRoute> GetRoutes()
        {
            var routes = new List<ProxyRoute>();

            var routingRules = _routingArbitrageClient.GetRoutes();

            foreach (var route in routingRules)
            {
                routes.Add(new ProxyRoute
                {
                    RouteId = route.FriendlyUri,
                    ClusterId = route.ServiceId,
                    Match = new ProxyMatch
                    {
                        Path = route.FriendlyUri + (route.StartsWith ? "{**catch-all}" : "")
                    }
                });
            }

            return routes;
        }

        public List<Cluster> GetClusters()
        {
            var clusters = new List<Cluster>();

            var services = _routingArbitrageClient.GetServices();

            foreach (var service in services)
            {
                clusters.Add(new Cluster
                {
                    Id = service.Id,
                    Destinations = new Dictionary<string, Destination>
                    {
                        {
                            "default", new Destination
                            {
                                Address = service.Host
                            }
                        }
                    }
                });
            }

            return clusters;
        }
    }
}