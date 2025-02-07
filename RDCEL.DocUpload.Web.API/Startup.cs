using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RDCEL.DocUpload.Web.API.Startup))]

namespace RDCEL.DocUpload.Web.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
                   
            ConfigureAuth(app);
        }
    }
}
