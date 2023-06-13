using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class ListJobInforReturn
    {
        public List<JobInfor> Scheduled { get; set; }
        public List<JobInfor> Rejected { get; set; }

        public ListJobInforReturn()
        {

        }

        public ListJobInforReturn(List<JobInfor> scheduled, List<JobInfor> rejected)
        {
            Scheduled = scheduled;
            Rejected = rejected;
        }
    }
}
