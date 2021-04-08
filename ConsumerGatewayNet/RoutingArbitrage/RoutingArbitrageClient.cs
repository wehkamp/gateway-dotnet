using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace ConsumerGatewayNet.RoutingArbitrage
{
    public class RoutingArbitrageClient
    {
        private static List<RoutingRule> _rules;
        
        private HttpClient _httpClient;

        public RoutingArbitrageClient()
        {
            _httpClient = new HttpClient();
            
            var response = _httpClient.GetStringAsync("https://routing-arbitrage.trusted.nl.wehkamp.prod.blaze.ps/v1/routes").GetAwaiter().GetResult();

            _rules = JsonConvert.DeserializeObject<List<RoutingRule>>(response);
            
            Filter(_rules);
        }

        public List<RoutingRule> GetRoutes()
        {
            return _rules;
        }

        public List<RoutingService> GetServices()
        {
            return _rules.GroupBy(r => r.ServiceId).Select(r => r.First()).Select(r => new RoutingService
            {
                Id = r.ServiceId,
                Host = $"https://{r.ServiceId}.trusted.nl.wehkamp.prod.blaze.ps/"
            }).ToList();
        }

        private void Filter(List<RoutingRule> routingRules)
        {
            routingRules.RemoveAll(route => route.FriendlyUri.Contains("wehkamp.nl") || route.FriendlyUri.Contains("?") || route.FriendlyUri.Contains("//"));
        }
    }
}