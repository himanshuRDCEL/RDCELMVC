using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RDCEL.Zoho.Integration.Sync;

namespace RDCEL.Zoho.Integration
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            System.Net.ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            UTC_BridgeZohoSync UTCSync = new UTC_BridgeZohoSync();

            string REGISTRY_KEY = ConfigurationManager.AppSettings["RegistryKey"].ToString(); 
            string REGISTY_VALUE = ConfigurationManager.AppSettings["RegistryValue"].ToString();
            if (Convert.ToInt32(Microsoft.Win32.Registry.GetValue(REGISTRY_KEY, REGISTY_VALUE, 0)) == 0)
            {
                //UTCSync.MoveItemDataToDB();

                //Change the value since the program has run once now
                Microsoft.Win32.Registry.SetValue(REGISTRY_KEY, REGISTY_VALUE, 1, Microsoft.Win32.RegistryValueKind.DWord);
            }
            else
            {
                 UTCSync.ProcessPriceMasterInfo();
                 //UTCSync.ProcessPincodeMasterInfo();

                // UTCSync.ProcessEVCApprovedInfo();
                // UTCSync.ProcessSponsorInfo();
            }
        }

       
        
    }

}
