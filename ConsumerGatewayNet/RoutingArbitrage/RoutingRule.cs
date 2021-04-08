namespace ConsumerGatewayNet.RoutingArbitrage
{
    public class RoutingRule
    {
        public string FriendlyUri { get; set; }
        
        public bool StartsWith { get; set; }
        
        public string ServiceId { get; set; }
    }
}