using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Controllers
{
    public class LocationWithImagesController : ApiController
    {
        //// GET: api/LocationWithImages
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public images<image> Get(string SearchFor = "_all")
        {
            clsMainProgram cls = new clsMainProgram();
            images<image> imagelist = cls.SearchImagesPerLocation(SearchFor);
            return imagelist;

            

        }

        // POST: api/LocationWithImages
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LocationWithImages/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LocationWithImages/5
        public void Delete(int id)
        {
        }
    }
}
