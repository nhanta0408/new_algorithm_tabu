﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Model
{
    public class MaterialInfo
    {
        public MaterialInfo(string code, string name, int minimumQuantity, int currentQuantity)
        {
            this.code = code;
            this.name = name;
            this.minimumQuantity = minimumQuantity;
            this.currentQuantity = currentQuantity;
        }

        public string code { get; set; }
        public string name { get; set; }
        public int minimumQuantity { get; set; }
        public int currentQuantity { get; set; }

    }
}
