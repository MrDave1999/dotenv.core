using DotEnv.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DotEnv.Example.AspNetFramework.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var reader = new EnvReader();
            var baseUrl = reader["APP_BASE_URL"];
            return baseUrl;
        }
    }
}
