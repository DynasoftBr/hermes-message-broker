using System.Threading.Tasks;
using Dynasoft.Hermes.Api.Application.Mediator.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dynasoft.Hermes.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1", Deprecated = false)] // futura API depreciada
    [Route("api/v{version:apiVersion}/queue")]
    public class QueueController : Controller
    {
        private readonly IMediator _mediator;
        public QueueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageQueueCommand input )
        {
            var result = await _mediator.Send(input);
            return Ok(result);
        }

        [HttpPost("createQueue")]
        public async Task<IActionResult> CreateQueue([FromBody] CreateQueueCommand input)
        {
            var result = await _mediator.Send(input);
            return Ok(result);
        }
    }
}   
