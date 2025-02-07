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
    public class OrderStatusDataContract
    {
        public OrderStatusDataContract(bool status, string message, object orderId)
        {
            Status = status;
            Message = message;
            OrderId = orderId;
        }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string Message { get; set; }        
        [DataMember]
        public object OrderId { get; set; }
    }
}
