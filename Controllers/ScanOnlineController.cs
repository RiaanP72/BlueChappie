using BlueChappie.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            string status = cls.GetStatus(ClientRequestId.Replace("-", ""));
            return status;
        }
       
        // POST: api/ScanOnline
        public async Task Post(string Location, String ClientRequestId)
        {

            clsMainProgram cls = new clsMainProgram();
            cls.doSyncWork(Location, ClientRequestId.Replace("-", ""));
            StatisticsService ss=new StatisticsService();
            await ss.NotifyUpdates(ClientRequestId.Replace("-", ""));

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
