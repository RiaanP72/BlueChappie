using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(string emailaddress,string password)
        {
            //string _retval = "";
            //clsMainProgram cls = new clsMainProgram();
            //_retval = cls.login(emailaddress, password);
            //return _retval;
            return "wtf?'";
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
