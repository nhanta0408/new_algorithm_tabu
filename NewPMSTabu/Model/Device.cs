using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class Device
    {
        public string code { get; set; }
        public EDeviceType deviceType { get; set; }
        public WorkingTime[] workingTimes { get; set; }
        public List<WorkingTime> maintenanceTimes { get; set; }

        public Device(string code, EDeviceType deviceType, WorkingTime[] workingTimes, List<WorkingTime> maintenanceTimes)
        {
            this.code = code;
            this.deviceType = deviceType;
            this.workingTimes = workingTimes;
            this.maintenanceTimes = maintenanceTimes;
        }
    }
}
