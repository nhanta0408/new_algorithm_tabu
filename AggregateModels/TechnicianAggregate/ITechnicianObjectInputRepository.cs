using TabuSearchImplement.AggregateModels.WorkAggregate;

namespace TabuSearchImplement.AggregateModels.TechnicianAggregate
{
    public interface ITechnicianObjectInputRepository
    {
        TechnicianObjectInput Add(TechnicianObjectInput technician);
    }
}
