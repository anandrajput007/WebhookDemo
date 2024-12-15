﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhookDemo.Services.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string WebhookUrl { get; set; }
    }
}
