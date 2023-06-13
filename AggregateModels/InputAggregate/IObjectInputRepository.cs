using TabuSearchImplement.AggregateModels.InputAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;

namespace TabuSearchImplement.AggregateModels.InputAggregate
{
    public interface IObjectInputRepository
    {
        ListJobInforReturn Implement(ObjectInput input);
    }
}
