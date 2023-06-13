using static TabuSearchImplement.Constant;

namespace TabuSearchImplement.AggregateModels.WorkAggregate
{
    public class WorkObjectInput
    {
        public string id { get; set; }
        public string priority { get; set; }
        public string deviceCode { get; set; }
        public string problem { get; set; }
        public DateTime dueDate { get; set; }
        public string estProcessTime { get; set; }
        public MaterialOnWork[]? materials { get; set; }

        public WorkObjectInput()
        {

        }

        public WorkObjectInput(string id, string priority, string deviceCode, string problem, DateTime dueDate, string estProcessTime, MaterialOnWork[]? materials)
        {
            this.id = id;
            this.priority = priority;
            this.deviceCode = deviceCode;
            this.problem = problem;
            this.dueDate = dueDate;
            this.estProcessTime = estProcessTime;
            this.materials = materials;
        }
    }
}
