using MediatR;
using TabuSearchImplement.AggregateModels.InputAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement.Commands.Inputs;

namespace TabuSearchImplement.Commands.Inputs
{
    public class AddInputHandler : IRequestHandler<AddInputCommand, ListJobInforReturn>
    {
        private readonly IObjectInputRepository _objectInputRepository;

        public AddInputHandler(IObjectInputRepository objectInputRepository)
        {
            _objectInputRepository = objectInputRepository;
        }

        public Task<ListJobInforReturn> Handle(AddInputCommand request, CancellationToken cancellationToken)
        {
            ListJobInforReturn newListJobInfor = _objectInputRepository.Implement(request.input);

            return Task.FromResult(newListJobInfor);
        }
    }
}
