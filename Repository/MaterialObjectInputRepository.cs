using TabuSearchImplement.AggregateModels.MaterialAggregate;

namespace TabuSearchImplement.Repository
{
    public class MaterialObjectInputRepository : IMaterialObjectInputRepository
    {
        public static List<MaterialObjectInput> listMaterialObjects = new List<MaterialObjectInput>();
        public MaterialObjectInput Add(MaterialObjectInput material)
        {
            listMaterialObjects.Add(material);
            return material;
        }
    }
}
