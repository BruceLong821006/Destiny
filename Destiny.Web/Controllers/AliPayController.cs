using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Destiny.Web.Controllers
{
    public class AliPayController : ApiController
    {
        // GET: api/AliPay
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AliPay/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AliPay
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/AliPay/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AliPay/5
        public void Delete(int id)
        {
        }
    }
}
