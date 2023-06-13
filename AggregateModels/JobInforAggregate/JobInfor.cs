using Microsoft.VisualBasic;
using System.Data;
using System.Globalization;
using TabuSearchImplement.AggregateModels.WorkAggregate;
using static TabuSearchImplement.Constant;

namespace TabuSearchImplement.AggregateModels.JobInforAggregate
{
    public class JobInfor
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Device { get; set; }
        public string Work { get; set; }
        public string Technician { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartPlannedDate { get; set; }
        public DateTime EndPlannedDate { get; set; }
        public int EstProcessTime { get; set; }
        public MaterialOnWork[]? Materials { get; set; }
        public int[] ArrayFail { get; set; } = new int[5];
        public JobInfor() { }

        public JobInfor(int id, string device, string work, string technician, DateTime dueDate, DateTime startPlannedDate, DateTime endPlannedDate)
        {
            Id = id;
            Device = device;
            Work = work;
            Technician = technician;
            DueDate = dueDate;
            StartPlannedDate = startPlannedDate;
            EndPlannedDate = endPlannedDate;
        }

        public JobInfor(int id, int priority, string device, string work, string technician, DateTime dueDate, DateTime startPlannedDate, DateTime endPlannedDate, int estProcessTime, MaterialOnWork[]? materials, int[] arrayFail)
        {
            Id = id;
            Priority = priority;
            Device = device;
            Work = work;
            Technician = technician;
            DueDate = dueDate;
            StartPlannedDate = startPlannedDate;
            EndPlannedDate = endPlannedDate;
            EstProcessTime = estProcessTime;
            Materials = materials;
            ArrayFail = arrayFail;
        }
    }

    public class GetData
    {
        public static List<JobInfor> dataJobInfor(DataTable workTable)
        {
            List<JobInfor> listJobInfors = new List<JobInfor>();
            int i = 0;
            foreach (DataRow workRow in workTable.Rows)
            {
                JobInfor jobInfor = new JobInfor();
                jobInfor.Id = i++;
                jobInfor.Priority = int.Parse(workRow["Priority"].ToString());
                jobInfor.Device = workRow["Device"].ToString();
                jobInfor.Work = workRow["Work"].ToString();
                jobInfor.Technician = "";
                jobInfor.DueDate = DateTime.ParseExact(workRow["DueDate"].ToString(), "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                jobInfor.StartPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                jobInfor.EndPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                jobInfor.EstProcessTime = int.Parse(workRow["ExecutionTime"].ToString());
                listJobInfors.Add(jobInfor);
            }
            return listJobInfors;
        }
    }
}
