using MediatR;
using TabuSearchImplement.AggregateModels.TechnicianAggregate;

namespace TabuSearchImplement.Commands.Technicians
{
    public class AddTechnicianHandler : IRequestHandler<AddTechnicianCommand, TechnicianInputs>
    {
        private readonly ITechnicianObjectInputRepository _technicianObjectInputRepository;

        public AddTechnicianHandler(ITechnicianObjectInputRepository technicianObjectInputRepository)
        {
            _technicianObjectInputRepository = technicianObjectInputRepository;
        }

        public Task<TechnicianInputs> Handle(AddTechnicianCommand request, CancellationToken cancellationToken)
        {
            var newListTechnician = new List<TechnicianObjectInput>();
            foreach (TechnicianObjectInput technicianObject in request.technicians.JsonInput)
            {
                var technician = _technicianObjectInputRepository.Add(technicianObject);
                newListTechnician.Add(technician);
            }

            TechnicianInputs technicianInputs = new TechnicianInputs(newListTechnician.ToArray());
            return Task.FromResult(technicianInputs);
        }
    }
}
