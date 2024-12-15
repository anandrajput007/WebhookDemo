namespace WebhookDemo.Services.Models
{
    public class SimulateEvent
    {
        public string EventType { get; set; }
        public Dictionary<string, object> Payload { get; set; }
    }
}
