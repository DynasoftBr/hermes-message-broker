using Dynasoft.Hermes.Api.Application.DTO;
using MediatR;
using Newtonsoft.Json.Linq;

namespace Dynasoft.Hermes.Api.Application.Mediator.Commands
{
    public class SendMessageQueueCommand : IRequest<MessageQueueDto>
    {
        public string RoutingKey { get; set; }

        public JObject Content { get; set; }
    }
}
