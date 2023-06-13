using TabuSearchImplement.AggregateModels.MaterialAggregate;

namespace TabuSearchImplement.AggregateModels.MaterialAggregate
{
    public class MaterialInputs
    {
        public MaterialObjectInput[]? JsonInput { get; set; }

        public MaterialInputs() { }
        public MaterialInputs(MaterialObjectInput[]? jsonInput)
        {
            JsonInput = jsonInput;
        }
    }
}
