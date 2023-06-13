using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public enum ESpecializedGroup
    {
        mechanics,
        electrics,
        multi
    }

    public enum EDeviceType
    {
        bigInjection,
        smallInjection,
        mold
    }

    public class Technician
    {
        public Technician(string? id, string? name, int? skillLevel, ESpecializedGroup specializedGroup, EDeviceType deviceType, WorkingTime[] workingTimes, WorkingTime[]? overTime)
        {
            this.id = id;
            this.name = name;
            this.skillLevel = skillLevel;
            this.specializedGroup = specializedGroup;
            this.deviceType = deviceType;
            this.workingTimes = workingTimes;
            this.overTime = overTime;
        }

        public string? id { get; set; }

        public string? name { get; set; }
        public int? skillLevel { get; set; }
        public ESpecializedGroup specializedGroup { get; set; }
        public EDeviceType deviceType { get; set; }

        public WorkingTime[] workingTimes { get; set; }
        public WorkingTime[]? overTime { get; set; }

    }
}
