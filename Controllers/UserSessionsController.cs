using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlueChappie;
using System.Data;

namespace BlueChappie.Controllers
{
    public class UserSessionsController : ApiController
    {
        // GET: api/UserSessions

        public string Get(String sessionid)
        {
            clsMainProgram cls = new clsMainProgram();
            return cls.ReadDataSet("").Tables[0].Rows[0].Field<string>("userid");
        }

        // POST: api/UserSessions
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserSessions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserSessions/5
        public void Delete(int id)
        {
        }
    }
}
