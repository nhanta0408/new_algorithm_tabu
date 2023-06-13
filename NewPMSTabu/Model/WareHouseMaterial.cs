using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class WarehouseMaterial
    {
        public MaterialInfo MaterialInfo { get; set; }

        public decimal CurrentQuantity { get; set; }

        public bool IsEnough { get; set; }

        public decimal? RequestingQuantity { get; set; }
        public DateTime ExpectedDate { get; set; }

        public WarehouseMaterial(MaterialInfo materialInfor, decimal currentQuantity, bool isEnough, decimal? requestingQuantity, DateTime expectedDate)
        {
            MaterialInfo = materialInfor;
            CurrentQuantity = currentQuantity;
            IsEnough = isEnough;
            RequestingQuantity = requestingQuantity;
            ExpectedDate = expectedDate;
        }

        public WarehouseMaterial()
        {
        }
    }
}
