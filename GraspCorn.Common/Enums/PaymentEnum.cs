using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum TransactionTypeEnum
    {
        [Description("Cr")]
        Cr=1,
        [Description("Dr")]
        Dr=2,
    }

    public enum ModuletypeEnum
    {
        [Description("EVC")]
        EVC=1,
        [Description("ABB")]
        ABB=2,
    }
}
