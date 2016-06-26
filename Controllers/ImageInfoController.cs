using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Controllers
{
    public class ImageInfoController : ApiController
    {
        // GET: api/ImageInfo
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ImageInfo/5
        public image Get(string imgGUID)
        {
            clsMainProgram cls = new clsMainProgram();
            return cls.SearchImage(imgGUID);
        }

        // POST: api/ImageInfo
        // Creates and image and returns the id for the saved image
        // if the image exist, the id of the exixting image is retrurned
        // The uniqueness is detrermend by the sourceURL
        // If imgGUID == Null it will be created automatically
        public void Post([FromBody]image img)
        {
          
        }

        // PUT: api/ImageInfo/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE: api/ImageInfo/5
        public void Delete(int id)
        {
        }
    }
}
