using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;

namespace TabuSearchImplement.Repository
{
    public class WareHouseMaterialObjectInputRepository : IWareHouseMaterialObjectInputRepository
    {
        public static List<WareHouseMaterialObjectInput> listWareHouseMaterialObjects = new List<WareHouseMaterialObjectInput>();
        public WareHouseMaterialObjectInput Add(WareHouseMaterialObjectInput wareHouseMaterial)
        {
            listWareHouseMaterialObjects.Add(wareHouseMaterial);
            return wareHouseMaterial;
        }
    }
}
