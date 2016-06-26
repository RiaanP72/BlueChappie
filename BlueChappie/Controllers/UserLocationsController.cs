using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlueChappie.Controllers
{
    public class UserLocationsController : ApiController
    {
        // GET: api/UserLocations
    
        public Boolean Get(string userID,string tKey)
        {

            clsMainProgram cls = new clsMainProgram();
            return cls.setFavouriteLocation(userID, tKey);
        }

        // POST: api/UserLocations
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserLocations/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserLocations/5
        public void Delete(int id)
        {
        }
    }
}
