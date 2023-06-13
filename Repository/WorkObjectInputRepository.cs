using TabuSearchImplement.AggregateModels.WorkAggregate;

namespace TabuSearchImplement.Repository
{
    public class WorkObjectInputRepository : IWorkObjectInputRepository
    {
        public static List<WorkObjectInput> listWorkObjects = new List<WorkObjectInput>();
        public WorkObjectInput Add(WorkObjectInput work)
        {
            listWorkObjects.Add(work);
            return work;
        }
    }
}
