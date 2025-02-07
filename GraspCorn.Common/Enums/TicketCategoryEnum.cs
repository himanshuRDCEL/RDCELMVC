using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum TicketCategoryEnum

    {
        [Description("SRRQ")]
        ServiceRequest,
        [Description("ZBRK")]
        Breakdown,
        [Description("ZFRE")]
        FreeInstallation,
        [Description("ZNBR")]
        NonBreakdown,
        [Description("ZRMS")]
        RMSInstance,
    }
}
