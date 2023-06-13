using MediatR;
using Microsoft.AspNetCore.Mvc;
using TabuSearchImplement.AggregateModels.InputAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement.Commands.Inputs;

namespace TabuSearchImplement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InputsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ListJobInforReturn> Post(ObjectInput input)
        {
            return await _mediator.Send(new AddInputCommand(input));
        }
    }
}
