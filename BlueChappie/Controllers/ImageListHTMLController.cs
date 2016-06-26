using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlueChappie.Controllers
{
    public class ImageListHTMLController : ApiController
    {
        // GET: api/ImageListHTML
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ImageListHTML/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ImageListHTML
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ImageListHTML/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ImageListHTML/5
        public void Delete(int id)
        {
        }
    }
}
