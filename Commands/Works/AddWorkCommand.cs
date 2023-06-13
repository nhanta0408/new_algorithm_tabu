using MediatR;
using TabuSearchImplement.AggregateModels.WorkAggregate;

namespace TabuSearchImplement.Commands.Works
{
    public record AddWorkCommand(WorkInputs works) : IRequest<WorkInputs>;
}
