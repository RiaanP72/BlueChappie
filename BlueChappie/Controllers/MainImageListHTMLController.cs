using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Controllers
{
    public class MainImageListHTMLController : ApiController
    {
        // GET: api/MainImageListHTML
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MainImageListHTML/5
        public string Get(String Location)
        {
            string _retval = "";
            clsMainProgram cls = new clsMainProgram();
            images<image> imagelist = new images<image>();
            imagelist = cls.SearchImagesPerLocation(Location);

            
            int i = 0;
            string clickevent = "";
            char kwot = (char)34;
            char crlf = (char)13;
            int strLen = 0;
            int maxTextLenth = 31;
            _retval += "<table width=" + kwot + "100%" + kwot + " style=" + kwot + "width:100%" + kwot + ">";
            foreach (image img in imagelist)
            {
                i++;
                if (i == 1) {
                    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                    strLen = img.title.Length;
                    if (strLen > maxTextLenth) { strLen = maxTextLenth; }
                    _retval += "<tr><td><img class=" + kwot + "imagelist" + kwot + " onmouseover='cursor:pointer' onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnail240x250Base64Encoded + "" + kwot + " /> <p class=" + kwot + "imagename" + kwot + ">" + img.title.Replace("`", "'").Substring(0,strLen) + "</p></td>" + crlf;
                }
                if (i == 2)
                {
                    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                    strLen = img.title.Length;
                    if (strLen > maxTextLenth) { strLen = maxTextLenth; }
                    _retval += "<td><img class=" + kwot + "imagelist" + kwot + " onmouseover='cursor:pointer' onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnail240x250Base64Encoded + "" + kwot + " /> <p class=" + kwot + "imagename" + kwot + ">" + img.title.Replace("`", "'").Substring(0, strLen) + "</p></td>" + crlf;
                }
                if (i == 3)
                {
                    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                    strLen = img.title.Length;
                    if (strLen > maxTextLenth) { strLen = maxTextLenth; }
                    _retval += "<td><img class=" + kwot + "imagelist" + kwot + " onmouseover='cursor:pointer' onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnail240x250Base64Encoded + "" + kwot + " /> <p class=" + kwot + "imagename" + kwot + ">" + img.title.Replace("`", "'").Substring(0, strLen) + "</p></td>" + crlf;
                }
                if (i == 4)
                {
                    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                    strLen = img.title.Length;
                    if (strLen > maxTextLenth) { strLen = maxTextLenth; }
                    _retval += "<td><img class=" + kwot + "imagelist" + kwot + " onmouseover='cursor:pointer' onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnail240x250Base64Encoded + "" + kwot + " /> <p class=" + kwot + "imagename" + kwot + ">" + img.title.Replace("`", "'").Substring(0, strLen) + "</p></td></tr>" + crlf;
                    i = 0;
                }


                //if (i == 1)
                //{
                //    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                //    _retval += "<td><table><tr><td><img class=" + kwot + "imagelist" + kwot + " onmouseover='cursor:pointer' onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnail250x250Base64Encoded + "" + kwot + " /></td></tr><tr><td> <p class=" + kwot + "imagename" + kwot + ">" + img.title.Replace("`", "'") + "</p></td></tr></table></td></tr>" + crlf;
                //    i = 0;
                //}
                //else
                //{

                //    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                //    _retval += "<tr><td><table><tr><td><img class=" + kwot + "imagelist" + kwot + " onmouseover='cursor:pointer' onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnail250x250Base64Encoded + "" + kwot + " /></td></tr><tr><td> <p class=" + kwot + "imagename" + kwot + ">" + img.title.Replace("`", "'") + "</p></td></tr></table></td>" + crlf;

                //    i++;
                //}

            }
            _retval += "</table>";
            return _retval;
        }

        // POST: api/MainImageListHTML
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/MainImageListHTML/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MainImageListHTML/5
        public void Delete(int id)
        {
        }
    }
}
