using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.MasterModel
{
    public class PinCodeDataContract
    {
        public List<ZipCodes> ZipCodes { get; set; }
    }

    public class ZipCodes
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
    }

    //[DataContract]
    //[Serializable]
    //public class CheckpointDatabaseDataContract
    //{

    //    public CheckpointDatabaseDataContract(object checkpoint_Database)
    //    {
    //        Checkpoint_Database = checkpoint_Database;
    //    }

    //    [DataMember]
    //    public object Checkpoint_Database { get; set; }
    //}

}
