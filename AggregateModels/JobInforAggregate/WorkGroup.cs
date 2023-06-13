namespace TabuSearchImplement.AggregateModels.JobInforAggregate
{
    public class WorkGroup
    {
        public string? id { get; set; }

        public string? name { get; set; }
        public int? minimumSkillLevel { get; set; }
        public ESpecializedGroup specializedGroup { get; set; }

    }
}
