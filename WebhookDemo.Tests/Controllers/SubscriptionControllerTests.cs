using Microsoft.AspNetCore.Mvc;
using Moq;
using WebhookDemo.Controllers;
using WebhookDemo.Services.Interfaces;
using WebhookDemo.Services.Models;

namespace WebhookDemo.Tests.Controllers
{
    public class SubscriptionControllerTests
    {
        private readonly Mock<ISubscriptionService> _subscriptionServiceMock;
        private readonly SubscriptionController _controller;
        private readonly string _webhookKey = string.Empty;

        public SubscriptionControllerTests()
        {
            _subscriptionServiceMock = new Mock<ISubscriptionService>();
            _controller = new SubscriptionController(_subscriptionServiceMock.Object);
            _webhookKey = "4d542ac8-1844-4b7c-82b9-5376e28e8555";
        }

        [Fact]
        public void Subscribe_ShouldReturnOk()
        {
            // Arrange
            var subscription = new Subscription
            {
                EventType = "OrderCreated",
                WebhookUrl = $"https://webhook.site/{_webhookKey}"
            };
            _subscriptionServiceMock
                .Setup(s => s.AddSubscription(subscription))
                .Returns((Subscription sub) => sub);

            // Act
            var result = _controller.Subscribe(subscription);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSubscription = Assert.IsType<Subscription>(okResult.Value);
            Assert.Equal(subscription.EventType, returnedSubscription.EventType);
            Assert.Equal(subscription.WebhookUrl, returnedSubscription.WebhookUrl);
        }

        [Fact]
        public void GetSubscriptions_ShouldReturnSubscriptions()
        {
            // Arrange
            var subscriptions = new List<Subscription>
        {
            new Subscription { EventType = "OrderCreated", WebhookUrl = $"https://webhook.site/{_webhookKey}" },
            new Subscription { EventType = "OrderShipped", WebhookUrl = $"https://webhook.site/{_webhookKey}" }
        };

            _subscriptionServiceMock.Setup(s => s.GetSubscriptions()).Returns(subscriptions);

            // Act
            var result = _controller.GetSubscriptions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSubscriptions = Assert.IsType<List<Subscription>>(okResult.Value);
            Assert.Equal(2, returnedSubscriptions.Count);
        }

        [Fact]
        public async Task SimulateEvent_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var simulateEvent = new SimulateEvent
            {
                EventType = "OrderCreated",
                Payload = new Dictionary<string, object>
            {
                { "orderId", "12345" },
                { "customerName", "John Doe" },
                { "orderAmount", 250.75 }
            }
            };

            _subscriptionServiceMock.Setup(s => s.SimulateEvent(simulateEvent)).ReturnsAsync(true);

            // Act
            var result = await _controller.SimulateEvent(simulateEvent);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Event sent successfully", okResult.Value);
        }
    }
}
