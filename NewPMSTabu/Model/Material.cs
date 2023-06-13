using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class Material
    {
        public Material(MaterialInfo? materialInfo, int? requiredQuantity)
        {
            this.materialInfo = materialInfo;
            this.requiredQuantity = requiredQuantity;
        }

        public Material()
        {
        }

        public MaterialInfo? materialInfo { get; set; }
        public int? requiredQuantity { get; set; }
    }
}
