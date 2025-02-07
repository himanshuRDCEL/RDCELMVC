using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum SubTypeEnum

    {
        [Description("I0011")]
        ThirtyTwoNewProduct,
        [Description("I0012")]
        ThirtyThreeDeInstallation,
        [Description("I0013")]
        ThirtyFourReInstallation,
        [Description("I0014")]
        ThirtyFiveTansitDamage,
        [Description("I0015")]
        ThirtySixComponentDamage,
        [Description("I0016")]
        ThirtySevenRMS,
        [Description("I0017")]
        SixtyTwoOverhaul,
        [Description("I0018")]
        SixtyTestOperation,
        [Description("I0019")]
        FIR,
        [Description("I0020")]
        Demo,
        [Description("I0021")]
        AMCEnquiry,
        [Description("I0022")]
        PreSalesInspection,
        [Description("I0023")]
        ThirtyEightSaleEnquiry,
        [Description("I0024")]
        SixtyThreeHealthCheckUp,
        [Description("I0025")]
        ThirtyNineQualityCheck,
        [Description("I0026")]
        Others,
        [Description("I0027")]
        ZeroZeroDoesNotHeatOrCool,
        [Description("I0028")]
        TenDoesNotOperate,
        [Description("I0029")]
        ElevenStopsImmediately,
        [Description("I0030")]
        TwelveSomeTimesStops,
        [Description("I0031")]
        ThirteenBreakerTrips,
        [Description("I0032")]
        TwentyGasLeak,
        [Description("I0033")]
        TwentyOneWaterLeak,
        [Description("I0034")]
        ThirtyAbnormalNoise,
        [Description("I0035")]
        ThirtyOneAbnormalOdour,
        [Description("I0036")]
        WaterLeakage,
        [Description("I0037")]
        NinetyNineOther,
    }
}
