using System;
using System.Collections.Generic;
using System.Text;

namespace Dynasoft.Hermes.Infrastructure.Messaging
{
    public class MessageEnvelop
    {
        public string RoutingKey { get; set; }

        public object Content { get; set; }
    }
}
