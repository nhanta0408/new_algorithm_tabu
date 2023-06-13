using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
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
