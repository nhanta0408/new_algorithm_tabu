using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class GetData
    {
        public static List<JobInfor> dataJobInfor(List<Work> listWorkAvailable)
        {
            List<JobInfor> listJobInfors = new List<JobInfor>();

            int i = 0;
            foreach(Work work in listWorkAvailable)
            {
                JobInfor jobInfor = new JobInfor();
                jobInfor.Id = i++;
                jobInfor.Priority = work.priority;
                jobInfor.Device = work.device;
                jobInfor.WorkGroup = work.workGroup;
                jobInfor.Technician = null;
                jobInfor.DueDate = work.dueDate;
                jobInfor.StartPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                jobInfor.EndPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                jobInfor.EstProcessTime = double.Parse(work.executionTime.ToString());
                listJobInfors.Add(jobInfor);
            }

            return listJobInfors;
        }
    }
}
