using TabuSearchImplement.AggregateModels.TechnicianAggregate;

namespace TabuSearchImplement.AggregateModels.JobInforAggregate
{
    public class Device
    {
        public Device(string code, EDeviceType deviceType, WorkingTime[] workingTimes)
        {
            this.code = code;
            this.deviceType = deviceType;
            this.workingTimes = workingTimes;
        }

        public string code { get; set; }

        public EDeviceType deviceType { get; set; }

        public WorkingTime[] workingTimes { get; set; }
    }
}
