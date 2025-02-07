using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum VoucherStatus
    {
        [Description("Capture")]
        Capture = 2,

        [Description("Reedem")]
        Reedem = 3,
    }
}
