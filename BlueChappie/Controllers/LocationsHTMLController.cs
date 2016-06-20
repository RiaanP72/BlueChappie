﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Controllers
{
    public class LocationsHTMLController : ApiController
    {

        // GET: api/LocationsHTML/5
        public string Get(String UserId = "")
        {
            string _retval = "";
            clsMainProgram cls = new clsMainProgram();
            locationsWithThumbNails<locationWithThumbNails> Locations = cls.getLocationsWithThumbNails(UserId);
            char kwot = (char)34;
            char crlf = (char)13;
            _retval += "<table>";
            foreach (locationWithThumbNails lcn in Locations)
            {
                _retval += "<tr><td><p class=" + kwot + "locationsp" + kwot + " onclick=" + kwot + "imgSelectLocation('" + lcn.tKey + "')" + kwot + ">" + lcn.tag + "</p>";

                if (!UserId.Equals("undefined"))
                {
                    if (lcn.isFavourite)
                    {
                        _retval += "<img src=" + kwot + "images/favRem.png" + kwot + " class=" + kwot + "fav" + kwot + " onclick=" + kwot + "addLocationToFavourite('" + lcn.tKey + "')" + kwot + " id=" + kwot + lcn.tKey + kwot + "  style=" + kwot + "cursor:pointer" + kwot + " />";
                    }
                    else
                    {
                        _retval += "<img src=" + kwot + "images/favAdd.png" + kwot + " class=" + kwot + "fav" + kwot + " onclick=" + kwot + "addLocationToFavourite('" + lcn.tKey + "')" + kwot + " id=" + kwot + lcn.tKey + kwot + "  style=" + kwot + "cursor:pointer" + kwot + " />";
                    }

                }
                _retval += "</td><td><p class=" + kwot + "locationsp" + kwot + " onclick=" + kwot + "imgSelectLocation('" + lcn.tKey + "')" + kwot + ">" + lcn.counter + "</td><td>";




                _retval += "<table><tr>";
                int i = 0;

                foreach (image img in lcn.locationimages)
                {

                    _retval += "<td><img  onclick=" + kwot + "imgSelectLocation('" + lcn.tKey + "')" + kwot + " class=" + kwot + "locationsimg" + kwot + " onmousehover='cursor:pointer' src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnailBase64Encoded + "" + kwot + "  style=" + kwot + "cursor:pointer" + kwot + " /></td>" + crlf;

                    i++;
                    if (i > 13)
                    {
                        break;
                    }
                }

                _retval += "</tr></table></td></tr>";
            }
            _retval += "</table>";
            return _retval;
        }

        // POST: api/LocationsHTML
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LocationsHTML/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LocationsHTML/5
        public void Delete(int id)
        {
        }
    }
}
