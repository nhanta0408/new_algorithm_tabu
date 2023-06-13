using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;

namespace TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate
{
    public class WareHouseMaterialInputs
    {
        public WareHouseMaterialObjectInput[]? JsonInput { get; set; }

        public WareHouseMaterialInputs() { }
        public WareHouseMaterialInputs(WareHouseMaterialObjectInput[]? jsonInput)
        {
            JsonInput = jsonInput;
        }
    }
}
