using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlueChappieControlls
{
    [Designer("CustomImageList.Tableformat, CustomTable"), DefaultProperty("Text"), ToolboxData("<{0}:wucImages1 runat=server></{0}:wucImages1>")]
    class wucImages : WebControl
    {
        public wucImages() : base(HtmlTextWriterTag.Span)
        { }
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write("Custom Contentscxdsffg xdf");
            base.RenderContents(writer);
        }

    }
}
