﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDCEL.DocUpload.Web.API.Models
{
    public class LoginModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}