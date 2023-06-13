using TabuSearchImplement.AggregateModels.DeviceAggregate;

namespace TabuSearchImplement.AggregateModels.DeviceAggregate
{
    public interface IDeviceObjectInputRepository
    {
        DeviceObjectInput Add(DeviceObjectInput device);
    }
}
