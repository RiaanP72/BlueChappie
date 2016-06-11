using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml;
using Elasticsearch.Net;
using BlueChappie.Models;
using static BlueChappie.Models.MainApplicationModels;
using System.Net;

namespace BlueChappie
{
    class clsMainProgram
    {
        public DataTable SeachFor(String searchFor)
        {
            DataSet dataSet = ReadDB("SELECT TOP 2 * FROM tbl_USR_Client WITH (nolock) WHERE vch_CNT_Name LIKE '%" + searchFor + "%' OR vch_CNT_Number LIKE '%" + searchFor + "%'");
            DataTable dataTable = dataSet.Tables[0];
            return dataTable;
        }


        public PlainList<plainList>  lstProviceList(){
            var node = new Uri("http://10.0.0.10:9200/bluechappie/regions");
            var config = new Elasticsearch.Net.ConnectionConfiguration(node)
                .PrettyJson(true);
            var client = new ElasticLowLevelClient(config);
            var query = new { query = new { term = new { systype = "province" } } };
            dynamic result = client.Search<Object>(query);
            var provincelist = new PlainList<plainList>();
            for (int i = 0; i < result.Body.hits.total; i++)
            {
                provincelist.Add(new plainList( result.Body.hits.hits[i]._source.name));
            }
            return provincelist;
              
        }


        public System.Data.DataSet ReadDB(String SQLCommand)
        {
            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection("Database=ProTrack;Server=localhost;uid=ProTrack2;pwd=protrack123;Connect Timeout=30;Min Pool Size=5;Max Pool Size=900;");
            System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.DataSet dataSet = new System.Data.DataSet();
            sqlDataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand(SQLCommand, sqlConnection);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            sqlDataAdapter.Fill(dataSet, "SQL Data Set");
            return dataSet;
        }
        public string tbl2JSON(DataTable dataTable)
        {
            string JSONresult;
            JSONresult = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
            return JSONresult;

        }
        public string info2XML(string dataTable)
        {
            System.Xml.XmlElement serializedXmlElement = null;

            try
            {
                System.IO.MemoryStream memoryStream = new MemoryStream();
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(dataTable.GetType());
                xmlSerializer.Serialize(memoryStream, dataTable);
                memoryStream.Position = 0;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(memoryStream);
                serializedXmlElement = xmlDocument.DocumentElement;
            }
            catch (Exception e)
            {
                //logging statements. You must log exception for review
            }

            return serializedXmlElement.InnerXml.ToString();

        }
        public string SyncImages(string tag = "durban") {
            //
            // API call derived from: https://www.flickr.com/services/api/explore/flickr.photos.search
            //
            //  Imnportant site to know about: http://www.programmableweb.com/
            //

            string _response = readAPI("https://api.flickr.com/services/rest/?method=flickr.photos.search&tags=" + tag + "&text=landmark&safe_search=&format=rest&api_key=7431acca741fc97b174a5b4c8110b68c",tag);
            return _response;
        }
        public string readAPI(string apiURL, String tag)
        {
            string completeUrl = apiURL;
            string _retval = "";
            System.Net.WebRequest request = WebRequest.Create(completeUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            XmlReader xmlreader = XmlReader.Create(new StringReader(responseFromServer));
            while (xmlreader.Read())
            {
                switch (xmlreader.NodeType)
                {
                    case XmlNodeType.Element:
                        _retval = (xmlreader.Name);
                        if (Equals(xmlreader.Name, "photo"))
                        {
                            image imageinfo = new image();
                            string imagepth = "https://farm{farm-id}.staticflickr.com/{server-id}/{id}_{secret}.jpg".Normalize().Replace("{farm-id}", xmlreader.GetAttribute("farm")).Replace("{server-id}", xmlreader.GetAttribute("server")).Replace("{id}", xmlreader.GetAttribute("id")).Replace("{secret}", xmlreader.GetAttribute("secret"));
                            imageinfo.title = xmlreader.GetAttribute("title");
                            imageinfo.localURL = imagepth;
                            imageinfo.sourceURL = imagepth;
                            imageinfo.source = apiURL;
                            imageinfo.keywords = tag;
                            SaveImage(imageinfo);
                        }
                        break;
                    case XmlNodeType.Text:
                        _retval = (xmlreader.Value);
                        break;
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.ProcessingInstruction:
                        _retval = (xmlreader.Name);//, xmlreader.Value);
                        break;
                    case XmlNodeType.Comment:
                        _retval = (xmlreader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        //  _retval =WriteFullEndElement();
                        break;
                }
            }
            return "";
        }
        public void SaveImage(image img) {
            var node = new Uri("http://10.0.0.10:9200/bluechappie/imagelib");
            var config = new ConnectionConfiguration(node);
            var client = new ElasticLowLevelClient(config);
            string imgInfo = Newtonsoft.Json.JsonConvert.SerializeObject(img);
            PostData<object> postingData = imgInfo;
            dynamic result0 = client.DoRequest<object>(HttpMethod.PUT, img.imgGUID, postingData);
        }

        
        
    }
}
