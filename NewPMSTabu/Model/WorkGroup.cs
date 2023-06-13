using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class WorkGroup
    {
        public string? id { get; set; }

        public string? name { get; set; }
        public int? minimumSkillLevel { get; set; }
        public ESpecializedGroup specializedGroup { get; set; }
      
    }
}
