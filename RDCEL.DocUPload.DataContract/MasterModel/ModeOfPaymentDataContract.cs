using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.MasterModel
{
    public class ModeOfPaymentDataContract
    {
        public List<ModeofPaymentName> ModeOfPayment { get; set; }
    }

    public class ModeofPaymentName
    {
        public int Id { get; set; }
        public string ModeofPayment { get; set; }
    }

}
