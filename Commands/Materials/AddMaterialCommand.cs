using MediatR;
using TabuSearchImplement.AggregateModels.MaterialAggregate;

namespace TabuSearchImplement.Commands.Materials
{
    public record AddMaterialCommand(MaterialInputs materials) : IRequest<MaterialInputs>;
}
