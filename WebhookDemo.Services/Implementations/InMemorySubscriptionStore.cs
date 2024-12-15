using WebhookDemo.Services.Interfaces;
using WebhookDemo.Services.Models;

namespace WebhookDemo.Services.Implementations
{
    public class InMemorySubscriptionStore : IInMemorySubscriptionStore
    {
        private readonly List<Subscription> _subscriptions = new();

        public Subscription AddSubscription(Subscription subscription)
        {
            subscription.Id = _subscriptions.Count + 1;
            _subscriptions.Add(subscription);
            return subscription;
        }

        public List<Subscription> GetAllSubscriptions()
        {
            return _subscriptions;
        }

        public List<Subscription> GetSubscriptionsByEventType(string eventType)
        {
            return _subscriptions
                .Where(s => s.EventType.Equals(eventType, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
