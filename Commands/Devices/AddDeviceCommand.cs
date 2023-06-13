using MediatR;
using TabuSearchImplement.AggregateModels.DeviceAggregate;

namespace TabuSearchImplement.Commands.Devices
{
    public record AddDeviceCommand(DeviceInputs devices) : IRequest<DeviceInputs>;
}
