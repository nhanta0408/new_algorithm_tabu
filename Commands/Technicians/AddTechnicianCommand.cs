using MediatR;
using TabuSearchImplement.AggregateModels.TechnicianAggregate;

namespace TabuSearchImplement.Commands.Technicians
{
    public record AddTechnicianCommand(TechnicianInputs technicians) : IRequest<TechnicianInputs>;
}
