using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum SourceofCallEnum

    {
        [Description("Customer Call")]
        CustomerCall = 101,
        [Description("E-Commerce")]
        ECommerce = 105,
        [Description("Email")]
        Email = 111,
        [Description("Letter")]
        Letter = 121,
        [Description("Social Media")]
        SocialMedia = 131,
        [Description("Legal Desk")]
        LegalDesk = 141,
        [Description("Web Form")]
        WebForm = 151,
        [Description("Dealer")]
        Dealer = 161,
        [Description("DAIPL Engineer")]
        DAIPLEngineer = 171,
        [Description("DAIPL Management")]
        DAIPLManagement = 181,
        [Description("DIL / Overseas")]
        DILOrOverseas = 191,
        [Description("MD office")]
        MDOffice = 201,
        [Description("Architect / Consultant")]
        ArchitectOrConsultant = 211,
        [Description("Key Account Customer")]
        KeyAccountCustomer = 221,
    }
}
