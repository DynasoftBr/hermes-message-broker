using System;
using System.Collections.Generic;
using System.Text;

namespace Dynasoft.Hermes.Infrastructure.Messaging
{
    public interface IMessagePublisher
    {
        string CreateMessage(string queueName, string companyName);
        void Publish(IEnumerable<MessageEnvelop> messages);
        void Publish(string exchange, bool durableExchange, string exchangeType, IEnumerable<MessageEnvelop> messages);
    }
}
