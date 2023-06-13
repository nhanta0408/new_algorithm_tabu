using NewPMSTabu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Utils
{
    public static class Ratio
    {
        public const int fineLeadRatio = 1;
        public const int fineLagRatio = 2;
        public const int fineOverTimeRatio = 1;
        public const int differrentLevel = 1000;

        public static int leadLag(Work work)
        {
            var plannedStart = work.plannedStart;
            var dueDate = work.dueDate;
            var executionTime = work.executionTime;

            if (plannedStart != null)
            {
                DateTime plannedFinish = (DateTime)plannedStart?.AddMinutes(executionTime);
                TimeSpan different = dueDate.Subtract(plannedFinish);
                var ratio = 0;
                if (different.Minutes > 0)
                {
                    //Lead
                    ratio = fineLeadRatio;
                }
                else
                {
                    ratio = fineLagRatio;
                }
                return different.Minutes * ratio;
            }
            return 0;
        }

        public static int fineOverTime(Technician technician)
        {
            var overTimes = technician.overTime;
            var duration = 0;
            if (overTimes != null)
            {
                foreach (var overTime in overTimes)
                {
                    duration = overTime.getMinutes();
                }
            }
            return duration * (technician.skillLevel ?? 1) * fineOverTimeRatio;

        }
    }
}
