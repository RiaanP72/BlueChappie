using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ctlLocations runat=server></{0}:ctlLocations>")]
    public class ctlLocations : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string UserId
        {
            get
            {
                String s = (String)ViewState["UserId"];
                return ((s == null) ? "" : s);
            }

            set
            {
                ViewState["UserId"] = value;
            }
        }
        


        public Boolean UserOnly { get; set; }
        //{
        //    private Boolean b = false;
        //  get
        //    {
        //        b = (Boolean)ViewState["UserOnly"];
        //        return (b);
        //    }

        //    set
        //    {
        //        ViewState["UserOnly"] = value;
        //    }
        //}
        protected override void RenderContents(HtmlTextWriter output)
        {
            string _retval = HtmlString();
            output.Write(_retval);
        }
        public string HtmlString()
        {
            string _retval = "";
            clsMainProgram cls = new clsMainProgram();
            locationsWithThumbNails<locationWithThumbNails> Locations = cls.getLocationsWithThumbNails(this.UserId);
            char kwot = (char)34;
            char crlf = (char)13;
            _retval += "<table>";
            foreach (locationWithThumbNails lcn in Locations)
            {
                    _retval += "<tr><td><p class=" + kwot + "locationsp" + kwot + " onclick=" + kwot + "imgSelectLocation('" + lcn.tKey + "')" + kwot + ">" + lcn.tag + "</p>";
                
                if (!this.UserId.Equals("")) {
                    if (lcn.isFavourite)
                    {
                        _retval += "<img src=" + kwot + "images/favRem.png" + kwot + " class=" + kwot + "fav" + kwot + " onclick=" + kwot + "addLocationToFavourite('" + lcn.tKey + "')" + kwot + " id=" + kwot + lcn.tKey + kwot + " />";
                }
                    else
                    {
                        _retval += "<img src=" + kwot + "images/favAdd.png" + kwot + " class=" + kwot + "fav" + kwot + " onclick=" + kwot + "addLocationToFavourite('" + lcn.tKey + "')" + kwot + " id=" + kwot + lcn.tKey + kwot + " />";
                    }

                }
                    _retval += "</td><td><p class=" + kwot + "locationsp" + kwot + " onclick=" + kwot + "imgSelectLocation('" + lcn.tKey + "')" + kwot + ">" + lcn.counter + "</td><td>";
                



                _retval += "<table><tr>";
                int i = 0;
               
                foreach (image img in lcn.locationimages)
                {

                    _retval += "<td><img  onclick=" + kwot + "imgSelectLocation('" + lcn.tKey + "')" + kwot + " class=" + kwot + "locationsimg" + kwot + " onmousehover='cursor:pointer' src=" + kwot + "data:image/jpeg;base64," + img.webImageThumbnailBase64Encoded + "" + kwot + " /></td>" + crlf;
                    
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
    }
}
