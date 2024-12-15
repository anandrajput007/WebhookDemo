using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhookDemo.Services.Models
{
    public class SimulateEvent
    {
        public string EventType { get; set; }
        public Dictionary<string, object> Payload { get; set; }
    }
}
