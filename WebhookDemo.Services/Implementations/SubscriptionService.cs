using System.Text.Json;
using System.Text;
using WebhookDemo.Services.Interfaces;
using WebhookDemo.Services.Models;
using WebhookDemo.Services.Util;

namespace WebhookDemo.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IInMemorySubscriptionStore _subscriptionStore;

        public SubscriptionService(IInMemorySubscriptionStore subscriptionStore)
        {
            _subscriptionStore = subscriptionStore;
        }

        public Subscription AddSubscription(Subscription subscription)
        {
            return _subscriptionStore.AddSubscription(subscription);
        }

        public List<Subscription> GetSubscriptions()
        {
            return _subscriptionStore.GetAllSubscriptions();
        }

        public async Task<bool> SimulateEvent(SimulateEvent simulateEvent)
        {
            AddEventLogs(simulateEvent);

            var subscriptions = _subscriptionStore.GetSubscriptionsByEventType(simulateEvent.EventType);

            if (subscriptions.Any())
            {
                foreach (var subscription in subscriptions)
                {
                    var httpClient = new HttpClient();
                    var payload = new
                    {
                        eventType = simulateEvent.EventType,
                        message = "This is a test event for " + simulateEvent.EventType
                    };

                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    await httpClient.PostAsync(subscription.WebhookUrl, content);
                }

                return true;
            }

            return false;
        }

        private void AddEventLogs(SimulateEvent simulateEvent)
        {
            EventLogStore.EventLogs.Add(new EventLogEntry
            {
                EventType = simulateEvent.EventType,
                Payload = JsonSerializer.Serialize(simulateEvent.Payload),
                Timestamp = DateTime.UtcNow
            });
        }

    }
}
