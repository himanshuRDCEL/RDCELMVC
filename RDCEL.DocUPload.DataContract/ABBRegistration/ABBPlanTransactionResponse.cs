using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ABBRegistration
{
   public class ABBPlanTransactionResponse
    {
        public string Message { get; set; }
        public string Response { get; set; }
        public int TransactionId { get; set; }
    }
}
