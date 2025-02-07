using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum ProductCatEnum
    {
        [Description("CookTop")]
        CookTop = 12,
    }
    public enum ProductTypeOldEnum
    {
        [Description("Split AC")]
        SplitAC = 10
    }
    public enum ProductTypeNewEnum
    {
        [Description("BPL")]
        BPL,
    }
}
