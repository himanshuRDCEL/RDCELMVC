using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.Common
{
 
    public class MailJetViewModel
    {
        public List<MailJetMessage> Messages { get; set; }
    }


    public class MailJetFrom
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class MailJetMessage
    {
        public MailJetFrom From { get; set; }
        public List<MailjetTo> To { get; set; }
        public string Subject { get; set; }
        public string TextPart { get; set; }
        public string HTMLPart { get; set; }
    }

   
    public class MailjetTo
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
