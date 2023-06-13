using TabuSearchImplement.AggregateModels.TechnicianAggregate;

namespace TabuSearchImplement.AggregateModels.TechnicianAggregate
{
    public class TechnicianInputs
    {
        public TechnicianObjectInput[]? JsonInput { get; set; }

        public TechnicianInputs() { }
        public TechnicianInputs(TechnicianObjectInput[]? jsonInput)
        {
            JsonInput = jsonInput;
        }
    }
}
