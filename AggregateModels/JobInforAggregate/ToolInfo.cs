namespace TabuSearchImplement.AggregateModels.JobInforAggregate
{
    public class ToolInfo
    {
        public ToolInfo(string code, string name, int currentQuantity)
        {
            this.code = code;
            this.name = name;
            this.currentQuantity = currentQuantity;
        }

        public string code { get; set; }
        public string name { get; set; }
        public int currentQuantity { get; set; }
    }
}
