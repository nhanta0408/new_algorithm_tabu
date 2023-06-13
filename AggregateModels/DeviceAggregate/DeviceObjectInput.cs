using TabuSearchImplement.AggregateModels.TechnicianAggregate;

namespace TabuSearchImplement.AggregateModels.DeviceAggregate
{
    public class DeviceObjectInput
    {
        public string? code { get; set; }
        public WorkingTime[]? workingTimes { get; set; }

        public DeviceObjectInput()
        {

        }

        public DeviceObjectInput(string? code, WorkingTime[]? workingTimes)
        {
            this.code = code;
            this.workingTimes = workingTimes;
        }
    }
}
