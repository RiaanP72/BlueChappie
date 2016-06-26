using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlueChappie.Controllers
{
    public class ScanOnlineController : ApiController
    {
        
        
        // GET: api/ScanOnline
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ScanOnline/5
        public String Get(String ClientRequestId)
        {
            clsMainProgram cls = new clsMainProgram();
            return cls.GetStatus(ClientRequestId.Replace("-",""));
        }
       
        // POST: api/ScanOnline
        public void Post(string Location, String ClientRequestId)
        {
            clsMainProgram cls = new clsMainProgram();
            cls.doSyncWork(Location, ClientRequestId.Replace("-", ""));
        }

        // PUT: api/ScanOnline/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ScanOnline/5
        public void Delete(int id)
        {
        }
    }
}
