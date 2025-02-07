using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ProductsPrices
{
    public class ProductsPricesListDataContract
    {
        public List<ProductsPricesDataContract> ProductsPricesDataContractList { get; set; }
    }

    public class ProductsPricesDataContract
    {
        public int CategoryId { get; set; }
        public string categoryName { get; set; }
        public List<ProductsFromTypePriceList> ProductsFromType { get; set; }
        public string SweetenerAmount { get; set; }
     
    }

    public class BrandPriceList
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Mid_Price { get; set; }
        public string Min_Price { get; set; }
        public string Scrap_Price { get; set; }
    }

    public class ProductsFromTypePriceList
    {
        public int id { get; set; }
        public int ProducttypeId { get; set; }
        public string name { get; set; }
        public List<BrandPriceList> Brand { get; set; }
    }

    public class Root
    {
       
    }

    public class ProductsPricesListDataContractSamsung
    {
        public List<ProductsPricesDataContractSamsung> ProductsPricesDataContractList { get; set; }
    }

    public class ProductsPricesDataContractSamsung
    {
        public int CategoryId { get; set; }
        public string categoryName { get; set; }
        public List<ProductsFromTypePriceList> ProductsFromType { get; set; }
      
    }
}
