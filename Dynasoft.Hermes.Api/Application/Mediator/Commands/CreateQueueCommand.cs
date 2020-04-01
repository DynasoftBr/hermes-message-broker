using Dynasoft.Hermes.Api.Application.DTO;
using MediatR;

namespace Dynasoft.Hermes.Api.Application.Mediator.Commands
{
    public class CreateQueueCommand : IRequest<MessageCreatedDto>
    {
        public string QueueName { get; set; }

        public string CompanyName { get; set; }
    }
}
