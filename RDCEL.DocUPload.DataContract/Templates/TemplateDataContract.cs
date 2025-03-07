﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.Templates
{
    public class TemplateDataContract
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public bool IsCertificateAvailable { get; set; }
        public bool IsInvoiceAvailable { get; set; }

        // Certificate Attachment
        public string AttachmentFileName { get; set; }
        public string AttachmentFilePath { get; set; }
        public string FileNameWithPath { get; set; }

        // Invoice Attachment
        public string InvAttachFileName { get; set; }
        public string InvAttachFilePath { get; set; }
        public string InvFileNameWithPath { get; set; }
        public bool IsInvoiceGenerated { get; set; }
    }
}
