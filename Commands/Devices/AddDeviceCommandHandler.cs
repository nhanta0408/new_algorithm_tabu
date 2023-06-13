using MediatR;
using TabuSearchImplement.AggregateModels.DeviceAggregate;
using TabuSearchImplement.Commands.Devices;

namespace TabuSearchImplement.Commands.Devices
{
    public class AddDeviceHandler : IRequestHandler<AddDeviceCommand, DeviceInputs>
    {
        private readonly IDeviceObjectInputRepository _deviceObjectInputRepository;

        public AddDeviceHandler(IDeviceObjectInputRepository deviceObjectInputRepository)
        {
            _deviceObjectInputRepository = deviceObjectInputRepository;
        }

        public Task<DeviceInputs> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
        {
            var newListDevice = new List<DeviceObjectInput>();
            foreach (DeviceObjectInput deviceObject in request.devices.JsonInput)
            {
                var device = _deviceObjectInputRepository.Add(deviceObject);
                newListDevice.Add(device);
            }

            DeviceInputs deviceInputs = new DeviceInputs(newListDevice.ToArray());
            return Task.FromResult(deviceInputs);
        }
    }
}
