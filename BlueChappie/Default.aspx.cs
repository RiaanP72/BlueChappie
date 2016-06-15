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
            //this.txtLocation.Text = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //clsMainProgram cls = new clsMainProgram();
            //cls.ReadDataSet("SELECT sobjects.name FROM sysobjects sobjects with (nolock) WHERE sobjects.xtype = 'U' and name <>'sysdiagrams' ORDER by 1 DESC");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            clsMainProgram cls = new clsMainProgram();
            BlueChappie.Models.BlueChappieModels.image img = new BlueChappie.Models.BlueChappieModels.image();
            img.imgGUID = "wsefdase";
            img.keyword = "ricyvlei";
            img.localURL = "localurl";
            img.sourceURL = "sourceurl";
            img.tag = "tag";
            img.title = "title";
            img.source = "dev";
            cls.SaveImage(img);


            //cls.SaveImage
        }

        protected void btnLookUpAndSync_Click(object sender, EventArgs e)
        {
            this.ctrlImages1.Location = this.txtLocation.Text;
            clsMainProgram cls = new clsMainProgram();
            cls.SyncImages(this.txtLocation.Text);
            this.ctrlImages1.Location = this.txtLocation.Text;
            //Response.Redirect("/api/values?SearchFor=" + this.TextBox1.Text);

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            
        }

        
        protected void Button4_Click(object sender, EventArgs e)
        {
            System.Data.DataTable xmlTable = new System.Data.DataTable();
            System.Xml.XmlElement serializedXmlElement = null;
            clsMainProgram cls = new clsMainProgram();
            images<image> imagelist = new images<image>();
            imagelist = cls.SearchImages();
            System.Data.DataSet xmlDataSet = new System.Data.DataSet();
            System.IO.MemoryStream memoryStream = new MemoryStream();
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(imagelist.GetType());
            xmlSerializer.Serialize(memoryStream, imagelist);
            memoryStream.Position = 0;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(memoryStream);
            serializedXmlElement = xmlDocument.DocumentElement;
            string XML = "<images>" + serializedXmlElement.InnerXml.ToString() + "</images>" ;
            xmlDataSet.ReadXml(new StringReader(XML));
            xmlTable = xmlDataSet.Tables[0];
            //this.Label1.Text = xmlTable.Rows[0]["imgGUID"].ToString();
            //this.Image1.ImageUrl = "data:image/jpeg;base64," + xmlTable.Rows[0]["webImageBase64Encoded"].ToString();
            //xmlTable.Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            //for (int i = 0; i < xmlDataSet.Tables[0].Rows.Count; i++) {
            //    //xmlTable.Rows[i]["Image"] = cls.Base64ToImage(xmlTable.Rows[i]["webImageBase64Encoded"].ToString());
            //}
            //xmlTable.AcceptChanges();
            //this.GridView1.DataSource = xmlTable;
            //this.GridView1.DataBind();
            //this.gr
            //foreach (image img in imagelist) 
            //{
            //    writer.Write("<tr><td>" + img.title + "</td><td>" + img.webImageBase64Encoded + "</td></tr>;");
            //}

        }
        //private void CreateImageColumn()
        //{
        //    GridViewImageColumn imageColumn;
        //    int columnCount = 0;
        //    do
        //    {
        //        Bitmap unMarked = blank;
        //        imageColumn = new DataGridViewImageColumn();

        //        //Add twice the padding for the left and  
        //        //right sides of the cell.
        //        imageColumn.Width = x.Width + 2 * bitmapPadding + 1;

        //        imageColumn.Image = unMarked;
        //        dataGridView1.Columns.Add(imageColumn);
        //        columnCount = columnCount + 1;
        //    }
        //    while (columnCount < 3);
        //}

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //clsMainProgram cls = new clsMainProgram();
            //e.Row.Cells[0]. = cls.Base64ToImage(e.Row.Cells[13].ToString());
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
                    }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            
        }
    }

    
    }
