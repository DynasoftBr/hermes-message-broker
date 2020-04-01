using System.Threading;
using System.Threading.Tasks;
using Dynasoft.Hermes.Api.Application.DTO;
using Dynasoft.Hermes.Infrastructure.Messaging;
using MediatR;

namespace Dynasoft.Hermes.Api.Application.Mediator.Commands
{
    public class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand, MessageCreatedDto>
    {
        private readonly IMessagePublisher _messagePublisher;

        public CreateQueueCommandHandler(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public Task<MessageCreatedDto> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
        {
            var routingQueue = _messagePublisher.CreateMessage(request.QueueName, request.CompanyName);

            return Task.FromResult(new MessageCreatedDto{RoutingKey = routingQueue});
        }
    }
}
