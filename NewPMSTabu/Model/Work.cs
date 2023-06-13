using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public enum ERejectedReason
    {
        leakWorkingTime,
        leakMaterial
    }
    public class Work
    {
        public string? id { get; set; }
        public int priority { get; set; }
        public string? name { get; set; }
        public WorkGroup? workGroup { get; set; }
        public Device device { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime? plannedStart { get; set; }
        public int executionTime { get; set; }
        public Material[]? materials { get; set; }
        public Tool[]? tools { get; set; }
        public bool isRejected { get; set; }
        public ERejectedReason? rejectedReason { get; set; }

        public Work(string? id, int priority, string? name, WorkGroup? workGroup, Device device, DateTime dueDate, DateTime? plannedStart, int executionTime, Material[]? materials, Tool[]? tools, bool isRejected, ERejectedReason? rejectedReason)
        {
            this.id = id;
            this.priority = priority;
            this.name = name;
            this.workGroup = workGroup;
            this.device = device;
            this.dueDate = dueDate;
            this.plannedStart = plannedStart;
            this.executionTime = executionTime;
            this.materials = materials;
            this.tools = tools;
            this.isRejected = isRejected;
            this.rejectedReason = rejectedReason;
        }
    }
}
