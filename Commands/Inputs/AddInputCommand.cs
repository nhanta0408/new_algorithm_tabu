using MediatR;
using TabuSearchImplement.AggregateModels.DeviceAggregate;
using TabuSearchImplement.AggregateModels.InputAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;

namespace TabuSearchImplement.Commands.Inputs
{
    public record AddInputCommand(ObjectInput input) : IRequest<ListJobInforReturn>;
}
