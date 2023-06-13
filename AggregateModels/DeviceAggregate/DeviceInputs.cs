using TabuSearchImplement.AggregateModels.DeviceAggregate;

namespace TabuSearchImplement.AggregateModels.DeviceAggregate
{
    public class DeviceInputs
    {
        public DeviceObjectInput[]? JsonInput { get; set; }

        public DeviceInputs() { }
        public DeviceInputs(DeviceObjectInput[]? jsonInput)
        {
            JsonInput = jsonInput;
        }
    }
}
