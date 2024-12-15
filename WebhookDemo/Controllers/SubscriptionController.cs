using Microsoft.AspNetCore.Mvc;
using WebhookDemo.Services.Interfaces;
using WebhookDemo.Services.Models;

namespace WebhookDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody] Subscription subscription)
        {
            var result = _subscriptionService.AddSubscription(subscription);
            return Ok(result);
        }

        [HttpGet("subscriptions")]
        public IActionResult GetSubscriptions()
        {
            var subscriptions = _subscriptionService.GetSubscriptions();
            return Ok(subscriptions);
        }

        [HttpPost("simulate")]
        public async Task<IActionResult> SimulateEvent([FromBody] SimulateEvent simulateEvent)
        {
            var result = await _subscriptionService.SimulateEvent(simulateEvent);
            return result ? Ok("Event sent successfully") : NotFound("No subscriptions found for the event type.");
        }
    }
}
