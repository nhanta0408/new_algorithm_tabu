using MediatR;
using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;
using TabuSearchImplement.Commands.WareHouseMaterials;

namespace TabuSearchImplement.Commands.WareHouseWareHouseMaterials
{
    public class AddWareHouseMaterialHandler : IRequestHandler<AddWareHouseMaterialCommand, WareHouseMaterialInputs>
    {
        private readonly IWareHouseMaterialObjectInputRepository _wareHouseMaterialObjectInputRepository;

        public AddWareHouseMaterialHandler(IWareHouseMaterialObjectInputRepository wareHouseMaterialObjectInputRepository)
        {
            _wareHouseMaterialObjectInputRepository = wareHouseMaterialObjectInputRepository;
        }

        public Task<WareHouseMaterialInputs> Handle(AddWareHouseMaterialCommand request, CancellationToken cancellationToken)
        {
            var newListWareHouseMaterial = new List<WareHouseMaterialObjectInput>();
            foreach (WareHouseMaterialObjectInput wareHouseMaterialObject in request.wareHouseMaterials.JsonInput)
            {
                var wareHouseMaterial = _wareHouseMaterialObjectInputRepository.Add(wareHouseMaterialObject);
                newListWareHouseMaterial.Add(wareHouseMaterial);
            }

            WareHouseMaterialInputs wareHouseMaterialInputs = new WareHouseMaterialInputs(newListWareHouseMaterial.ToArray());
            return Task.FromResult(wareHouseMaterialInputs);
        }
    }
}
