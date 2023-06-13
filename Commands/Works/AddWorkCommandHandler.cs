using MediatR;
using TabuSearchImplement.AggregateModels.WorkAggregate;

namespace TabuSearchImplement.Commands.Works
{
    public class AddWorkHandler : IRequestHandler<AddWorkCommand, WorkInputs>
    {
        private readonly IWorkObjectInputRepository _workObjectInputRepository;

        public AddWorkHandler(IWorkObjectInputRepository workObjectInputRepository)
        {
            _workObjectInputRepository = workObjectInputRepository;
        }

        public Task<WorkInputs> Handle(AddWorkCommand request, CancellationToken cancellationToken)
        {
            var newListWork = new List<WorkObjectInput>();
            foreach(WorkObjectInput workObject in request.works.JsonInput)
            {
                var work = _workObjectInputRepository.Add(workObject);
                newListWork.Add(work);
            }

            WorkInputs workInputs = new WorkInputs(newListWork.ToArray());
            return Task.FromResult(workInputs);
        }
    }
}
