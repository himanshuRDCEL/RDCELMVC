﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    [Authorize]
    public class WebApiController : ApiController
    {
        // GET api/WebApi  
        public IEnumerable<string> Get()
        {
            string userid = RequestContext.Principal.Identity.Name;

            return new string[] { "Hello REST API", "I am Authorized", userid };
        }

        // GET api/WebApi/5  
        public string Get(int id)
        {
            return "Hello Authorized API with ID = " + id;
        }

        // POST api/WebApi  
        public void Post([FromBody]string value)
        {
        }

        // PUT api/WebApi/5  
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/WebApi/5  
        public void Delete(int id)
        {
        }
    }
}
