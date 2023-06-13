using static TabuSearchImplement.Constant;

namespace TabuSearchImplement.AggregateModels.MaterialAggregate
{
    public class MaterialObjectInput
    {
        public int? id { get; set; }
        public int priority { get; set; }
        public string device { get; set; }
        public string work { get; set; }
        public DateTime dueDate { get; set; }
        public decimal estProcessTime { get; set; }
        public List<WareHouseMaterialClass> partList { get; set; }
        public List<int> sequencePartList { get; set; }
        public List<decimal> quantityPart { get; set; }

        public MaterialObjectInput()
        {

        }

        public MaterialObjectInput(int? id, int priority, string device, string work, DateTime dueDate, decimal estProcessTime, List<WareHouseMaterialClass> partList, List<int> sequencePartList, List<decimal> quantityPart)
        {
            this.id = id;
            this.priority = priority;
            this.device = device;
            this.work = work;
            this.dueDate = dueDate;
            this.estProcessTime = estProcessTime;
            this.partList = partList;
            this.sequencePartList = sequencePartList;
            this.quantityPart = quantityPart;
        }
    }
}
