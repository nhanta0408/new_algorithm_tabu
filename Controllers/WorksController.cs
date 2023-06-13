using MediatR;
using Microsoft.AspNetCore.Mvc;
using TabuSearchImplement.AggregateModels.WorkAggregate;
using TabuSearchImplement.Commands.Works;

namespace TabuSearchImplement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<WorkInputs> Post(WorkInputs works)
        {
            return await _mediator.Send(new AddWorkCommand(works));
        }
    }
}
