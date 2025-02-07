using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum ABBPlanEnum
    {
        [Description("Innvoice value")]
        BoschBasicValue=40000,
        [Description ("GstType")]
        GstExclusive=37,

        [Description("GstType")]
        GstInclusive = 36,

        [Description("MarginType")]
        MarginTypeFixed = 40,

        [Description("MarginType")]
        MarginTypePerc = 41,

        [Description("ERP")]
        ModuleERP = 20,
    }
}
