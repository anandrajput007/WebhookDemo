using WebhookDemo.Services.Interfaces;
using WebhookDemo.Services.Models;
using WebhookDemo.Services.ServiceFebric;

namespace WebhookDemo.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly InMemorySubscriptionStore _subscriptionStore;

        public SubscriptionService()
        {
            _subscriptionStore = new InMemorySubscriptionStore();
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
            var subscriptionsForEvent = _subscriptionStore.GetSubscriptionsByEventType(simulateEvent.EventType);

            if (subscriptionsForEvent.Count == 0)
                return false;

            using var httpClient = new HttpClient();
            foreach (var subscription in subscriptionsForEvent)
            {
                var payloadJson = System.Text.Json.JsonSerializer.Serialize(simulateEvent.Payload);
                var content = new StringContent(payloadJson, System.Text.Encoding.UTF8, "application/json");

                // Send POST request to Webhook URL
                await httpClient.PostAsync(subscription.WebhookUrl, content);
            }

            return true;
        }
    }
}
