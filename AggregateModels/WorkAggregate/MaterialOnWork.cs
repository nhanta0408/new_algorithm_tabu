using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;

namespace TabuSearchImplement.AggregateModels.WorkAggregate
{
    public class MaterialOnWork
    {
        public MaterialInforOnWork? materialInfo { get; set; }
        public string? quantity { get; set; }
    }

    public class MaterialInforOnWork
    {
        public string? code { get; set; }
        public string? name { get; set; }
    }
}
