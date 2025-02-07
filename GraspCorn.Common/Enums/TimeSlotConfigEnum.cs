using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum TimeSlotConfigEnum
    {
        [Description("4")]
        TotalAgents = 4,
        [Description("3")]
        CallPerHour = 5,
    }
}
