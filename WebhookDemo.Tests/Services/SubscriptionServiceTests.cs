using WebhookDemo.Services.Implementations;
using WebhookDemo.Services.Interfaces;
using WebhookDemo.Services.Models;

namespace WebhookDemo.Tests.Services
{
    public class SubscriptionServiceTests
    {
        private readonly IInMemorySubscriptionStore _subscriptionStore;
        private readonly SubscriptionService _subscriptionService;
        private readonly HttpClient _httpClient;
        private readonly string _webhookKey = string.Empty;

        public SubscriptionServiceTests()
        {
            _subscriptionStore = new InMemorySubscriptionStore();
            _httpClient = new HttpClient();
            _subscriptionService = new SubscriptionService(_subscriptionStore, _httpClient);
            _webhookKey = "4d542ac8-1844-4b7c-82b9-5376e28e8555";
        }

        [Fact]
        public void AddSubscription_ShouldAddSubscription()
        {
            // Arrange
            var subscription = new Subscription
            {
                EventType = "OrderCreated",
                WebhookUrl = $"https://webhook.site/{_webhookKey}"
            };

            // Act
            var result = _subscriptionService.AddSubscription(subscription);

            // Assert
            Assert.NotNull(result);
            var subscriptions = _subscriptionStore.GetAllSubscriptions();
            Assert.Single(subscriptions);
            Assert.Equal("OrderCreated", subscriptions.First().EventType);
        }

        [Fact]
        public void GetSubscriptions_ShouldReturnAllSubscriptions()
        {
            // Arrange
            _subscriptionStore.AddSubscription(new Subscription
            {
                EventType = "OrderCreated",
                WebhookUrl = $"https://webhook.site/{_webhookKey}"
            });
            _subscriptionStore.AddSubscription(new Subscription
            {
                EventType = "OrderShipped",
                WebhookUrl = $"https://webhook.site/{_webhookKey}"
            });

            // Act
            var subscriptions = _subscriptionService.GetSubscriptions();

            // Assert
            Assert.Equal(2, subscriptions.Count);
        }
    }
}
