using System.Collections.Generic;
using System.Text;
using Dynasoft.Hermes.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace Dynasoft.Hermes.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly RabbitSettings _rabbitSettings;
        private readonly IConnection _connection;

        public MessagePublisher(IOptions<RabbitSettings> rabbitSettings, IConnection connection)
        {
            _rabbitSettings = rabbitSettings.Value;
            _connection = connection;
        }

        public string CreateMessage(string queueName, string companyName)
        {
            var durableExchange = bool.Parse(_rabbitSettings.Publishers.DurableExchange);
            var exchangeType = _rabbitSettings.Publishers.ExchangeType;

            string result = $"hermes.{companyName.ToLower()}.{queueName.ToLower()}";
            string exchange = $"hermes.{companyName.ToLower()}";
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange, exchangeType, durableExchange);

                channel.QueueDeclare(queueName, true, false, false, null);

                channel.QueueBind(queueName, exchange, result, null);
            }

            return result;
        }

        public void Publish(IEnumerable<MessageEnvelop> messages)
        {
            //var exchange = _rabbitSettings.Publishers.ExchangeName;
            //var durableExchange = bool.Parse(_rabbitSettings.Publishers.DurableExchange);
            //var exchangeType = _rabbitSettings.Publishers.ExchangeType;

            using (var channel = _connection.CreateModel())
            {
                //channel.ExchangeDeclare(exchange, exchangeType, durableExchange);

                foreach (var message in messages)
                {
                    var json = JsonConvert.SerializeObject(message.Content, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    var body = Encoding.UTF8.GetBytes(json);

                    var rabbitData = message.RoutingKey.Split('.');

                    var exchange = $"{rabbitData[0]}.{rabbitData[1]}";

                    channel.BasicPublish(exchange: exchange,
                        routingKey: message.RoutingKey,
                        basicProperties: null,
                        body: body);
                }
            }
        }

        public void Publish(string exchange, bool durableExchange, string exchangeType, IEnumerable<MessageEnvelop> messages)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange, exchangeType, durableExchange);

                foreach (var message in messages)
                {
                    var json = JsonConvert.SerializeObject(message.Content, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: exchange,
                        routingKey: message.RoutingKey,
                        basicProperties: null,
                        body: body);
                }
            }
        }
    }
}
