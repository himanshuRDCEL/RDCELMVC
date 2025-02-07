using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.MasterModel
{
    public class PickupStatusDataContract
    {
        public List<StatusName> PickupStatus { get; set; }
    }

    public class StatusName
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }

}
