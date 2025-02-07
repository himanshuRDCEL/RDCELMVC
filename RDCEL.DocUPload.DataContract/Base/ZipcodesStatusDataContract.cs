using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.Base
{
    [DataContract]
    [Serializable]
    public class ZipCodesStatusDataContract
    {
        public ZipCodesStatusDataContract(bool status, string message, object zipCodes)
        {
            Status = status;
            Message = message;
            ZipCodes = zipCodes;
        }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string Message { get; set; }        
        [DataMember]
        public object ZipCodes { get; set; }
    }
}
