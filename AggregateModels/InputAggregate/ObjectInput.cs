using TabuSearchImplement.AggregateModels.DeviceAggregate;
using TabuSearchImplement.AggregateModels.TechnicianAggregate;
using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;
using TabuSearchImplement.AggregateModels.WorkAggregate;

namespace TabuSearchImplement.AggregateModels.InputAggregate
{
    public class ObjectInput
    {
        public WorkInputs works { get; set; }
        public DeviceInputs devices { get; set; }
        public TechnicianInputs technicians { get; set; }
        public WareHouseMaterialInputs wareHouseMaterials { get; set; }
        public DateTime firstDateStart { get; set; }
        public ObjectInput()
        {

        }

        public ObjectInput(WorkInputs works, DeviceInputs devices, TechnicianInputs technicians, WareHouseMaterialInputs wareHouseMaterials, DateTime firstDateStart)
        {
            this.works = works;
            this.devices = devices;
            this.technicians = technicians;
            this.wareHouseMaterials = wareHouseMaterials;
            this.firstDateStart = firstDateStart;
        }
    }
}
