using WebhookDemo.Services.Models;

namespace WebhookDemo.Services.Interfaces
{
    public interface IInMemorySubscriptionStore
    {
        Subscription AddSubscription(Subscription subscription);
        List<Subscription> GetAllSubscriptions();
        List<Subscription> GetSubscriptionsByEventType(string eventType);
    }
}
