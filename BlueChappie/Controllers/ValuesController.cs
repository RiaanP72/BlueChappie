using System.Collections.Generic;
using System.Web.Http;
using static BlueChappie.Models.BlueChappieModels;
using BlueChappie;
namespace BlueChappie.Controllers
{
  //  [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        public images<image> Get(string SearchFor="")
        {
            clsMainProgram cls = new clsMainProgram();
            //PlainList<plainList> ProvinceList = cls.lstProviceList();
            //return "{ PlainList: " +  Newtonsoft.Json.JsonConvert.SerializeObject( ProvinceList ) + "}";

            images<image> imagelist = cls.SearchImages(SearchFor);
            return imagelist;

            //return cls.SyncImages();

        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
