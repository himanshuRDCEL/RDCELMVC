using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RDCEL.Zoho.Book_Creator.Integration
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            System.Net.ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            CustomerWalletAmountSync walletync = new CustomerWalletAmountSync();
            
            string REGISTRY_KEY = ConfigurationManager.AppSettings["RegistryKey"].ToString(); 
            string REGISTY_VALUE = ConfigurationManager.AppSettings["RegistryValue"].ToString();
            //LibLogging.WriteErrorToDB("Program", "Execution Started - Step 1", null);
            //if (Convert.ToInt32(Microsoft.Win32.Registry.GetValue(REGISTRY_KEY, REGISTY_VALUE, 0)) == 0)
            //{
            //    //UTCSync.MoveItemDataToDB();

            //    //Change the value since the program has run once now
            //    Microsoft.Win32.Registry.SetValue(REGISTRY_KEY, REGISTY_VALUE, 1, Microsoft.Win32.RegistryValueKind.DWord);
            //}
            //else
            //{
                LibLogging.WriteErrorToDB("Program", "Execution Started - Step 1", null);
                walletync.ProcessContactInfo();
                //  UTCSync.ProcessPriceMasterInfo();
                LibLogging.WriteErrorToDB("Program", "Execution End - Step 1", null);
            //}
        }

       
        
    }

}
