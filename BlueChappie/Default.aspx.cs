using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueChappie;
using static BlueChappie.Models.BlueChappieModels;
using System.IO;
using System.Xml;
using System.Drawing;


namespace BlueChappie
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (Request.QueryString["location"] != null)
            {
                this.ctrlImages1.Location = Request.QueryString["location"];
                context.Session["location"] = Request.QueryString["location"];
            }
            if (Request.QueryString["user"] != null)
            {
                context.Session["user"] = Request.QueryString["user"];
                Response.Redirect("Default.aspx");
            }
            if (context.Session["location"] != null) { this.ctrlImages1.Location = context.Session["location"].ToString(); }
            if (context.Session["user"] != null) { setuser( context.Session["user"].ToString()); }
        }

        protected void setuser(string userId)
        {
            clsMainProgram cls = new clsMainProgram();
            user usr = cls.getUserDetails(userId);
            this.hvUserEmailAddress.Value = usr.emailaddress;
            this.hvUserId.Value = usr.userId;
            this.lblUserEmail.Text = usr.emailaddress;
            this.lblUserClick.Visible = false;
            this.lblUserEmail.Visible = true;
            this.ctlLocations1.UserId = usr.userId;
            this.ctlUserLocations1.UserId = usr.userId;
            
        }
  

        protected void btnLookUpAndSync_Click(object sender, EventArgs e)
        {
            if (this.txtLocation.Text.Length < 4)
            {
                Response.Write("<script>Alert('Please enter a search criteria of 4 or more letters')</script>");
            }
            else {
                clsMainProgram cls = new clsMainProgram();
                cls.SyncImages(this.txtLocation.Text);
                this.ctrlImages1.Location = this.txtLocation.Text;

            }
            
        }
        
        //protected void Button4_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable xmlTable = new System.Data.DataTable();
        //    System.Xml.XmlElement serializedXmlElement = null;
        //    clsMainProgram cls = new clsMainProgram();
        //    images<image> imagelist = new images<image>();
        //    imagelist = cls.SearchImages();
        //    System.Data.DataSet xmlDataSet = new System.Data.DataSet();
        //    System.IO.MemoryStream memoryStream = new MemoryStream();
        //    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(imagelist.GetType());
        //    xmlSerializer.Serialize(memoryStream, imagelist);
        //    memoryStream.Position = 0;
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.Load(memoryStream);
        //    serializedXmlElement = xmlDocument.DocumentElement;
        //    string XML = "<images>" + serializedXmlElement.InnerXml.ToString() + "<images>" ;
        //    xmlDataSet.ReadXml(new StringReader(XML));
        //    xmlTable = xmlDataSet.Tables[0];
           

        //}
       
    }

    
    }
