using WebhookDemo.Services.Models;

namespace WebhookDemo.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Subscription AddSubscription(Subscription subscription);
        List<Subscription> GetSubscriptions();
        Task<bool> SimulateEvent(SimulateEvent simulateEvent);
    }
}
