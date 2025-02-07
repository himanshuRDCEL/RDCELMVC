using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum BrandIdEnum
    {
        [Description("Bosch")]
        BoschBrand=2002,

        [Description("Other")]
        OtherBrand= 2008,
    }

    public enum ProductConditionEnum
    {
        [Description("Not Working")]
        NotWorking=4,

        [Description("Working")]
        Working = 1,
        [Description("Excellent")]
        Excellent=1,

        [Description("Good")]
        Good=2,
        [Description("Average")]
        Average = 3,
    }
}
