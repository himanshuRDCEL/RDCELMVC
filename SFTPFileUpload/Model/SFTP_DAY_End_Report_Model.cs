using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPFileUpload.Model
{
    public class SFTP_DAY_End_Report_Model
    {
        public string DateOfTRX { get; set; }
        public string PickUpStatus { get; set; }
        public string DateOfDevicePickUp { get; set; }
        public string TransactionId { get; set; }
        public int? StoreId { get; set; }
        public string VoucherCode { get; set; }
        public double? Amount { get; set; }
        public double? OEMBonusAmount { get; set; }
        public string StorePickUpAddress { get; set; }
        public string UTRNumber { get; set; }
    }
}
