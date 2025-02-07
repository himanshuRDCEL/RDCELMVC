using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum ProductCategoryEnum
    {
        [Description("Refrigerator")]
        Refrigerator = 1,
        [Description("Washing Machine")]
        WashingMachine = 2,
        [Description("Television")]
        Television = 3,
        [Description("Air Conditioner")]
        AirConditioner = 4,
        [Description("Dishwasher")]
        Dishwasher = 5,
    }
}
