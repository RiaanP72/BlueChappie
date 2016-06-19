using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Controllers
{
    public class LocationsWithThumbNailsController : ApiController
    {
        //// GET: api/LocationsWithThumbNails
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/LocationsWithThumbNails/5
        public locationsWithThumbNails<locationWithThumbNails> Get(string userID = "_all")
        {

            clsMainProgram cls = new clsMainProgram();
            locationsWithThumbNails<locationWithThumbNails> LocationsWithThumbNails = cls.getLocationsWithThumbNails(userID);
            return LocationsWithThumbNails;
        }

        // POST: api/LocationsWithThumbNails
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LocationsWithThumbNails/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LocationsWithThumbNails/5
        public void Delete(int id)
        {
        }
    }
}
