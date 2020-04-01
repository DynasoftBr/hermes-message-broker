using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dynasoft.Hermes.Api.Application.DTO;
using Dynasoft.Hermes.Infrastructure.Messaging;
using MediatR;

namespace Dynasoft.Hermes.Api.Application.Mediator.Commands
{
    public class SendMessageQueueCommandHandler : IRequestHandler<SendMessageQueueCommand, MessageQueueDto>
    {
        private readonly IMessagePublisher _messagePublisher;

        public SendMessageQueueCommandHandler(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public Task<MessageQueueDto> Handle(SendMessageQueueCommand request, CancellationToken cancellationToken)
        {
            var envList = new List<MessageEnvelop>()
            {
                new MessageEnvelop
                {
                    RoutingKey = request.RoutingKey,
                    Content = request.Content
                }
            };

            _messagePublisher.Publish(envList);

            return Task.FromResult(new MessageQueueDto());
        }
    }
}
