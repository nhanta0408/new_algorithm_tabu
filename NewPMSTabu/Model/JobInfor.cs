using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class JobInfor
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public Device? Device { get; set; }
        public WorkGroup? WorkGroup { get; set; }
        public Technician? Technician { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartPlannedDate { get; set; }
        public DateTime EndPlannedDate { get; set; }
        public double EstProcessTime { get; set; }
        public Material[]? Materials { get; set; }
        public ArrayFail? ArrayFail { get; set; }
        public JobInfor() { }

        public JobInfor(int id, Device? device, WorkGroup? workGroup, Technician? technician, DateTime dueDate, DateTime startPlannedDate, DateTime endPlannedDate)
        {
            Id = id;
            Device = device;
            WorkGroup = workGroup;
            Technician = technician;
            DueDate = dueDate;
            StartPlannedDate = startPlannedDate;
            EndPlannedDate = endPlannedDate;
        }

        public JobInfor(int id, int priority, Device? device, WorkGroup? workGroup, Technician? technician, DateTime dueDate, DateTime startPlannedDate, DateTime endPlannedDate, double estProcessTime, Material[]? materials, ArrayFail arrayFail)
        {
            Id = id;
            Priority = priority;
            Device = device;
            WorkGroup = workGroup;
            Technician = technician;
            DueDate = dueDate;
            StartPlannedDate = startPlannedDate;
            EndPlannedDate = endPlannedDate;
            EstProcessTime = estProcessTime;
            Materials = materials;
            ArrayFail = arrayFail;
        }
    }

    public class FirstDateStart
    {
        public static DateTime firstDateStart { get; set; }
    }

    public class ArrayFail
    {
        public bool IsOk { get; set; }
        public bool DeviceBreakingTimeNotAvailable { get; set; }
        public bool TechnicianWorkingTimeNotAvailable { get; set; }
        public bool DeviceDuplicatedJob { get; set; }
        public bool TechnicianDuplicatedJob { get; set; }

        public ArrayFail(bool isOk, bool deviceBreakingTimeNotAvailable, bool technicianWorkingTimeNotAvailable, bool deviceDuplicatedJob, bool technicianDuplicatedJob)
        {
            IsOk = isOk;
            DeviceBreakingTimeNotAvailable = deviceBreakingTimeNotAvailable;
            TechnicianWorkingTimeNotAvailable = technicianWorkingTimeNotAvailable;
            DeviceDuplicatedJob = deviceDuplicatedJob;
            TechnicianDuplicatedJob = technicianDuplicatedJob;
        }
    }
}
