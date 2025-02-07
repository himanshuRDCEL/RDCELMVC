using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum ProductTypeEnum
    {
        [Description("Room Split AC")]
        RoomAir = 01,
        [Description("Sky AC")]
        SkyAir = 02,
        [Description("Multi Split AC")]
        MultiSplit = 03,
        [Description("Ductable/Package")]
        DuctableOrPackage = 04,
        [Description("VRV")]
        VRV = 05,
        [Description("Chiller")]
        Chiller = 06,
        [Description("Air Purifier")]
        AirPurifier = 07,
        [Description("Oil Cooler")]
        OilCooler = 08,
        [Description("VRV Smart")]
        VRVSmart = 09,
        [Description("Room Window AC")]
        WindowAir = 10,
    }
}
