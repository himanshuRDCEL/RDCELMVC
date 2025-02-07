using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ProductTaxonomy
{
    public class BrandDataContract
    {
        public List<BrandName> Brand { get; set; }
    }

    public class BrandName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

  
}
