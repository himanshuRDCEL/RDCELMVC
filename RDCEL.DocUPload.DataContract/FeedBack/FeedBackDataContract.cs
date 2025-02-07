using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RDCEL.DocUpload.DataContract.FeedBack
{
    public class FeedBackDataContract
    {
        public string RatingNo { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int CustomerId { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int ExchangeOrderId { get; set; }
    }

    public class FeedBackView
    {
        public string LogoName { get; set; }
        public int CustId { get; set; }
        public string Comment { get; set; }
        public string ratingId { get; set; }
        public int QesId { get; set; }
        public int AnsId { get; set; }
        public string Question { get; set; }
        public string Question2 { get; set; }
        public string Answer { get; set; }
        public int ExchangeOrderId { get; set; }

    }
}
