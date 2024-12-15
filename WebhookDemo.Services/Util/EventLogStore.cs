using WebhookDemo.Services.Models;

namespace WebhookDemo.Services.Util
{
    public static class EventLogStore
    {
        public static List<EventLogEntry> EventLogs { get; set; } = new List<EventLogEntry>();
    }
}
