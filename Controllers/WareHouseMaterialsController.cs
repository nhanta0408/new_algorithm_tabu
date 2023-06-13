using MediatR;
using Microsoft.AspNetCore.Mvc;
using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;
using TabuSearchImplement.Commands.WareHouseMaterials;

namespace TabuSearchImplement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseMaterialsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WareHouseMaterialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<WareHouseMaterialInputs> Post(WareHouseMaterialInputs wareHouseMaterials)
        {
            return await _mediator.Send(new AddWareHouseMaterialCommand(wareHouseMaterials));
        }
    }
}
