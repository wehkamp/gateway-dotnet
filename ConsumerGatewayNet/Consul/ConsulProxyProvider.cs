using System;
using System.Collections.Generic;
using Yarp.ReverseProxy.Abstractions;
using Yarp.ReverseProxy.Abstractions.ClusterDiscovery.Contract;

namespace ConsumerGatewayNet.Consul
{
    public class ConsulProxyProvider
    {
        public List<ProxyRoute> GetRoutes()
        {
            return new()
            {
                new()
                {
                    RouteId = "winkelmand",
                    ClusterId = "basket-site",
                    Match = new ProxyMatch
                    {
                        Path = "/winkelmand/{**catch-all}"
                    }
                },
                new()
                {
                    RouteId = "cop",
                    ClusterId = "category-site",
                    Match = new ProxyMatch
                    {
                        Path = "/{[a-z-]+}/{[A-Z0-9]+}"
                    }
                },
                new()
                {
                    RouteId = "home",
                    ClusterId = "content-site",
                    Match = new ProxyMatch
                    {
                        Path = "/"
                    }
                }
            };
        }

        public List<Cluster> GetClusters()
        {
            return new()
            {
                new()
                {
                    Id = "basket-site",
                    LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                    HealthCheck = new HealthCheckOptions
                    {
                        Active = new ActiveHealthCheckOptions
                        {
                            Enabled = true,
                            Interval = TimeSpan.FromSeconds(10),
                            Timeout = TimeSpan.FromSeconds(5),
                            Policy = HealthCheckConstants.ActivePolicy.ConsecutiveFailures,
                            Path = "/status"
                        }
                    },
                    Destinations = new Dictionary<string, Destination>
                    {
                        {
                            "container-1",
                            new Destination
                            {
                                Address = $"http://10.200.10.112:31898/"
                            }
                        },
                        {
                            "container-2",
                            new Destination
                            {
                                Address = $"http://10.200.8.251:31384/"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "category-site",
                    Destinations = new Dictionary<string, Destination>
                    {
                        {
                            "default",
                            new Destination
                            {
                                Address = $"https://basket-site.trusted.nl.wehkamp.prod.blaze.ps/"
                            }
                        }
                    }
                }
            };
        }
    }
}