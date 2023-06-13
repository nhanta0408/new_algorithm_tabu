using TabuSearchImplement.AggregateModels.TechnicianAggregate;

namespace TabuSearchImplement.Repository
{
    public class TechnicianObjectInputRepository : ITechnicianObjectInputRepository
    {
        public static List<TechnicianObjectInput> listTechnicianObjects = new List<TechnicianObjectInput>();
        public TechnicianObjectInput Add(TechnicianObjectInput technician)
        {
            listTechnicianObjects.Add(technician);
            return technician;
        }
    }
}
