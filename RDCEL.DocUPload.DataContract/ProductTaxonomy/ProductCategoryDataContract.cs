using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ProductTaxonomy
{
    public class ProductCategoryDataContract
    {
        public List<ProductCategory> ProductsCategory { get; set; }
    }

    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        
    }
    public class BrandOBJ
    {
        public string brandIdOld { get; set; }
        public bool  flag { get; set; }
    }

}
