using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class WorkingTime
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }


        public WorkingTime(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public int getMinutes()
        {
            return To.Subtract(From).Minutes;
        }
    }
}
