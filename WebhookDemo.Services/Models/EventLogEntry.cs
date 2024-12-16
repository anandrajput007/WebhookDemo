namespace WebhookDemo.Services.Models
{
    public class EventLogEntry
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccessful { get; set; }

    }
}
