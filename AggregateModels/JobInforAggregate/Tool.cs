namespace TabuSearchImplement.AggregateModels.JobInforAggregate
{
    public class Tool
    {
        public Tool(ToolInfo toolInfo, int requiredQuantity)
        {
            this.toolInfo = toolInfo;
            this.requiredQuantity = requiredQuantity;
        }

        public ToolInfo toolInfo { get; set; }
        public int requiredQuantity { get; set; }
    }
}
