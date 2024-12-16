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
        private readonly HttpClient _httpClient;

        public SubscriptionService(IInMemorySubscriptionStore subscriptionStore, HttpClient httpClient)
        {
            _subscriptionStore = subscriptionStore;
            _httpClient = httpClient;
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
            var subscriptions = _subscriptionStore.GetSubscriptionsByEventType(simulateEvent.EventType);

            if (subscriptions.Any())
            {
                foreach (var subscription in subscriptions)
                {
                    var payload = new
                    {
                        eventType = simulateEvent.EventType,
                        message = "This is a test event for " + simulateEvent.EventType
                    };

                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(subscription.WebhookUrl, content);

                    if (response.IsSuccessStatusCode) AddEventLogs(simulateEvent, response.IsSuccessStatusCode);
                }

                return true;
            }

            return false;
        }

        private void AddEventLogs(SimulateEvent simulateEvent, bool responseStatus = false)
        {
            EventLogStore.EventLogs.Add(new EventLogEntry
            {
                EventType = simulateEvent.EventType,
                Payload = JsonSerializer.Serialize(simulateEvent.Payload),
                Timestamp = DateTime.UtcNow,
                IsSuccessful = responseStatus
            });
        }

    }
}
