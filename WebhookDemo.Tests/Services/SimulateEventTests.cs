using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using WebhookDemo.Services.Implementations;
using WebhookDemo.Services.Interfaces;
using WebhookDemo.Services.Models;

namespace WebhookDemo.Tests.Services
{
    public class SimulateEventTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly IInMemorySubscriptionStore _subscriptionStore;
        private readonly SubscriptionService _subscriptionService;
        private readonly HttpClient _httpClient;
        private readonly string _webhookKey = string.Empty;

        public SimulateEventTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _subscriptionStore = new InMemorySubscriptionStore();
            _httpClient = new HttpClient();
            _subscriptionService = new SubscriptionService(_subscriptionStore, _httpClient);
            _webhookKey = "4d542ac8-1844-4b7c-82b9-5376e28e8555";
        }

        [Fact]
        public async Task SimulateEvent_ShouldReturnTrue_WhenHttpCallIsSuccessful()
        {
            // Arrange
            var subscriptionStoreMock = new Mock<IInMemorySubscriptionStore>();
            subscriptionStoreMock
                    .Setup(s => s.GetSubscriptionsByEventType(It.IsAny<string>()))
                    .Returns(new List<Subscription>
                    {
                        new Subscription { EventType = "OrderCreated", WebhookUrl = $"https://webhook.site/{_webhookKey}" }
                    });

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"success\":true}")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

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

            var subscriptionService = new SubscriptionService(subscriptionStoreMock.Object, httpClient);

            // Act
            var result = await subscriptionService.SimulateEvent(simulateEvent);

            // Assert
            Assert.True(result);
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString().Contains("https://webhook.site")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

    }
}
