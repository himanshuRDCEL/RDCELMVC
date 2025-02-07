using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum BusinessUnitEnum
    {
        [Description("Bosch")]
        Bosch = 1,

        [Description("Samsung")]
        Samsung = 2,

        [Description("D2C")]
        D2C = 4,
        [Description("Onsitego")]
        Onsitego = 6,
        [Description("Diakin")]
        Diakin = 10,

        [Description("Universal")]
        Universal = 12,

        [Description("Nikshan")]
        Nikshan = 13,

        [Description("Lg")]
        Lg = 8,

        [Description("Pine Labs")]
        PineLabs = 11,

        [Description("Alliance")]
        Alliance = 9,
        [Description("Havells")]
        Havells = 15,
        [Description("BoschDecoline")]
        BoschDecoline = 16,

        [Description("WhirlPool")]
        WhirlPool = 17,
        //Added by Vk
        [Description("Relience")]
        Relience = 5,
    }
    public enum StoreCodeEnum 
    {

        [Description("MYG-APP-01")]
        Mygate=9,

        [Description("ALC-LOD-01")]
        LodhaGroup= 5658,
    }

}
