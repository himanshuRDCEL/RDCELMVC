using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum  OrderTypeEnum
    {

        [Description("ABB")]
        ABB = 16,

        [Description("Exchange")]
        Exchange = 17,
    }

    public enum ProductAgeEnum
    {
        [Description("ProductAge")]
        ProductAge=2002,
    }

    public enum VoucherTypeEnum
    {
        [Description("Cash")]
        Cash = 2,

        [Description("Discount")]
        Discount = 1,

        [Description("NoVoucher")]
        NoVoucher = 0,
    }
}
