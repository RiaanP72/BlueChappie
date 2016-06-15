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
    [ToolboxData("<{0}:ctrlImages runat=server></{0}:ctrlImages>")]
    public class ctrlImages : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Location
        {
            get
            {
                String s = (String)ViewState["Location"];
                return ((s == null) ? "Durban" : s);
            }

            set
            {
                ViewState["Location"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            string _retval = HtmlString();
            output.Write(_retval);
        }
        public string HtmlString() {
            string _retval = "";
            clsMainProgram cls = new clsMainProgram();
            images<image> imagelist = new images<image>();
            imagelist = cls.SearchImages(Location);

            _retval += "<table>";
            int i = 0;
            string clickevent = "";
            char kwot = (char)34;
            char crlf = (char)13;
            //crlf += (char)10;
            foreach (image img in imagelist)
            {
                if (i == 1) {
                    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                    _retval += "<td><table><tr><td><img width=" + kwot + "500px" + kwot + " class=" + kwot + "imagelist" + kwot + " onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageBase64Encoded + "" + kwot + " /></td></tr><tr><td> <p class=" + kwot + "imagename" + kwot + ">" + img.title + "</p></td></tr></table></td></tr>"  + crlf;
                    i = 0;
                } else {

                    clickevent = kwot + "getImgData('" + img.imgGUID + "');" + kwot;
                    _retval += "<tr><td><table><tr><td><img width=" + kwot + "500px" + kwot + " class=" + kwot + "imagelist" + kwot + " onclick=" + clickevent + " src=" + kwot + "data:image/jpeg;base64," + img.webImageBase64Encoded + "" + kwot + " /></td></tr><tr><td> <p class=" + kwot + "imagename" + kwot + ">" + img.title + "</p></td></tr></table></td>" + crlf;
                  //  _retval += "<tr><td><table><tr><td> <img width='500px' class='imagelist' onclick=" + clickevent + " src='data:image/jpeg;base64," + img.webImageBase64Encoded + "'                                                                /></td></tr><tr><td> <p class='imagename'>" + img.title + "                      </p></td></tr></table></td>" + crlf;
                    i++;
                    }
                    
            }
            _retval += "</table>";
            return _retval;
        }
    }
}
