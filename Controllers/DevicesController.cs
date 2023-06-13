using MediatR;
using Microsoft.AspNetCore.Mvc;
using TabuSearchImplement.AggregateModels.DeviceAggregate;
using TabuSearchImplement.Commands.Devices;

namespace TabuSearchImplement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DevicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<DeviceInputs> Post(DeviceInputs devices)
        {
            return await _mediator.Send(new AddDeviceCommand(devices));
        }
    }
}
