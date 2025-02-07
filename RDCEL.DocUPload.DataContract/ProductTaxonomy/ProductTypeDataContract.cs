using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ProductTaxonomy
{
     public class ProductTypeDataContract
     {
        public List<ProductType> ProductsType { get; set; }
     }

    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int ProductCatId { get; set; }
       
    }

}
