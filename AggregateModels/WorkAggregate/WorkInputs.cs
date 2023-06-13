﻿namespace TabuSearchImplement.AggregateModels.WorkAggregate
{
    public class WorkInputs
    {
        public WorkObjectInput[]? JsonInput { get; set; }

        public WorkInputs() { }
        public WorkInputs(WorkObjectInput[]? jsonInput)
        {
            JsonInput = jsonInput;
        }
    }
}
