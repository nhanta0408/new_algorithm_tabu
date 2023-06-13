using MediatR;
using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;

namespace TabuSearchImplement.Commands.WareHouseMaterials
{
    public record AddWareHouseMaterialCommand(WareHouseMaterialInputs wareHouseMaterials) : IRequest<WareHouseMaterialInputs>;
}
