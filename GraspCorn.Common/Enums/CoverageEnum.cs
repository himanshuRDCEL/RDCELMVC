using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum CoverageEnum

    {
        [Description("InWarranty")]
        InWarranty = 101,
        [Description("In AMC")]
        InAMC = 111,
        [Description("Out Of Warranty")]
        OutOfWarranty = 121,
        [Description("Compressor Warranty")]
        CompressorWarranty = 131,
    }

    public enum FormatTypeEnum
    {
        [Description("Dealer")]
        Dealer=1,

        [Description("Home")]
        Home = 2,
    }
}
