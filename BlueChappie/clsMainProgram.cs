using System;
using System.Data;
using System.IO;
using System.Xml;
using Elasticsearch.Net;
using static BlueChappie.Models.BlueChappieModels;
using System.Net;
using System.Drawing;
namespace BlueChappie
{
    class clsMainProgram
    {
        public BlueChappieSettings blueChappieSetttings = new BlueChappieSettings();
        public DataTable SeachFor(String searchFor)
        {
            DataSet dataSet = ReadDataSet("SELECT TOP 2 * FROM tbl_USR_Client WITH (nolock) WHERE vch_CNT_Name LIKE '%" + searchFor + "%' OR vch_CNT_Number LIKE '%" + searchFor + "%'");
            DataTable dataTable = dataSet.Tables[0];
            return dataTable;
        }


        public PlainList<plainList> lstProviceList()
        {
            var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/regions");
            var config = new Elasticsearch.Net.ConnectionConfiguration(node)
                .PrettyJson(true);
            var client = new ElasticLowLevelClient(config);
            var query = new { query = new { term = new { systype = "province" } } };
            dynamic result = client.Search<Object>(query);
            var provincelist = new PlainList<plainList>();
            for (int i = 0; i < result.Body.hits.total; i++)
            {
                provincelist.Add(new plainList(result.Body.hits.hits[i]._source.name));
            }
            return provincelist;

        }


        public System.Data.DataSet ReadDataSet(String SQLCommand)
        {
            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection("Database=BlueChappie;Server=localhost;uid=ProTrack2;pwd=protrack123;Connect Timeout=30;Min Pool Size=5;Max Pool Size=900;");
           // System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.DataSet dataSet = new System.Data.DataSet();
            sqlDataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand(SQLCommand, sqlConnection);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            sqlDataAdapter.Fill(dataSet, "SQL Data Set");
            return dataSet;
        }

        public void RunSQL(String SQLCommand)
        {
            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection("Database=BlueChappie;Server=localhost;uid=ProTrack2;pwd=protrack123;Connect Timeout=30;Min Pool Size=5;Max Pool Size=900;");
            sqlConnection.Open();
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(SQLCommand, sqlConnection);
            sqlCommand.ExecuteNonQuery();
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
               object x = e;
                //logging statements. You must log exception for review
            }

            return serializedXmlElement.InnerXml.ToString();

        }
        public string SyncImages(string tag = "durban", string keyword = "landmark")
        {
            //
            // API call derived from: https://www.flickr.com/services/api/explore/flickr.photos.search
            //
            //  Important site to know about: http://www.programmableweb.com/
            //
            // the Flickr API key : 12ba11510a21ba897117735df462cdca and secret (12d265d5e9d5d421) for this application

            string _response = readAPI("https://api.flickr.com/services/rest/?method=flickr.photos.search&tags=" + tag + "&text=" + keyword + "&safe_search=&format=rest&api_key=12ba11510a21ba897117735df462cdca&extras=description, date_upload, date_taken, owner_name, tags, description", tag, keyword);
            return _response;
        }
        public string readAPI(string apiURL, String tag,string keyword)
        {
            string completeUrl = apiURL;
            string _retval = "";
            System.Net.WebRequest request = WebRequest.Create(completeUrl);
            HttpWebResponse response;
            WebResponse imgStream;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception)
            {

                return "'Error' : 'Connect to API error'";
            }
            
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            XmlReader xmlreader = XmlReader.Create(new StringReader(responseFromServer));
            Guid _guid = Guid.NewGuid();
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
                            if (!CheckExsitingImageSourceURL(imagepth)) {
                            
                                _guid = Guid.NewGuid();
                                imageinfo.imgGUID = _guid.ToString();
                                imageinfo.description = xmlreader.GetAttribute("description");
                                imageinfo.title = xmlreader.GetAttribute("title");
                                imageinfo.localURL = imagepth;
                                imageinfo.sourceURL = imagepth;
                                imageinfo.source = apiURL;

                                imageinfo.tag = tag;
                                imageinfo.keyword = keyword;
                                imageinfo.dateHit = DateTime.Now.ToString().Replace("/","-");
                                imageinfo.dateTaken = xmlreader.GetAttribute("datetaken");
                                imageinfo.tags = xmlreader.GetAttribute("tags");
                                imageinfo.owner = xmlreader.GetAttribute("ownername");
                                imageinfo.origin = "flickr.com";

                                request = WebRequest.Create(imageinfo.sourceURL);
                                imgStream = request.GetResponse();
                                imageinfo.webImageBase64Encoded = ImageToBase64( Image.FromStream(imgStream.GetResponseStream()),System.Drawing.Imaging.ImageFormat.Jpeg);
                              //  imgStream.Close;

                            


                                SaveImage(imageinfo);
                            }
                            imageinfo = new image();
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
        public void SaveImage(image img)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                string strSQL = "IF (SELECT count(*) FROM imagelib with (nolock) where sourceURL='" + img.sourceURL + "')=0  INSERT INTO imagelib(imgGUID,keyword,localURL,source,sourceURL,tag,title,dateHit,description,tags,owner,origin,dateTaken,webImageBase64Encoded) VALUES(";
                strSQL += "'" + img.imgGUID + "',";
                strSQL += "'" + img.keyword + "',";
                strSQL += "'" + img.localURL + "',";
                strSQL += "'" + img.source + "',";
                strSQL += "'" + img.sourceURL + "',";
                strSQL += "'" + img.tag + "',";
                strSQL += "'" + img.title + "',";
                strSQL += "'" + img.dateHit + "',";
                strSQL += "'" + img.description + "',";
                strSQL += "'" + img.tags + "',";
                strSQL += "'" + img.owner + "',";
                strSQL += "'" + img.origin + "',";
                strSQL += "'" + img.dateTaken + "',";
                strSQL += "'" + img.webImageBase64Encoded + "')";
                RunSQL(strSQL);
            }
            else
            {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib");
                var config = new ConnectionConfiguration(node);
                var client = new ElasticLowLevelClient(config);
                string imgInfo = Newtonsoft.Json.JsonConvert.SerializeObject(img);
                PostData<object> postingData = imgInfo;
                dynamic result0 = client.DoRequest<object>(HttpMethod.PUT, img.imgGUID, postingData);
            }
                saveLocation(img.tag);
            
        }
        public void saveLocation(String location) {
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                string strSQL = "IF (SELECT count(*) FROM locationlib with (nolock) where location='" + location.ToLower() + "')=0  INSERT INTO locationlib(location,created) VALUES(";
                strSQL += "'" +location.ToLower() + "',";
                strSQL += "'" + DateTime.Now.ToShortTimeString() + "')";
                RunSQL(strSQL);
            }
            else
            {

                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/locationlib");
                var config = new ConnectionConfiguration(node);
                var client = new ElasticLowLevelClient(config);
                string imgInfo = Newtonsoft.Json.JsonConvert.SerializeObject(location);
                PostData<object> postingData = imgInfo;
                dynamic result0 = client.DoRequest<object>(HttpMethod.PUT, location, postingData);
            }
        }
        public bool CheckExsitingImageSourceURL(String url)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                if (ReadDataSet("SELECT count(*) FROM imagelib with (nolock) where sourceURL='" + url + "'").Tables[0].Rows[0].Field<int>(0) == 0)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }
            else { 
                string criteria = "";
                if (url != null)
                {
                    criteria = "_search?sourceURL=" + url.ToLower();
                }
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/"+criteria);
                var config = new ConnectionConfiguration(node)
                    .PrettyJson(true);
                var client = new ElasticLowLevelClient(config);
                var query = new { query = new { origin = "flickr.com" } };
                dynamic result = client.Search<Object>(query);
                try
                {
                    if (result.Body.hits.total > 0) { return true; } else { return false; }
                }
                catch (Exception)
                {
                    return false;

                }

            }
        }

        public images<image> SearchImages(string searchtag = "durban")
        {
            images<image> imagelist = new images<image>();
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table = ReadDataSet("SELECT * FROM imagelib with (nolock) where tag='" + searchtag + "'").Tables[0];
                image img = new image();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    img = new image();
                    img.imgGUID = table.Rows[i].Field<string>("imgGUID");
                    img.keyword = table.Rows[i].Field<string>("keyword");
                    img.localURL = table.Rows[i].Field<string>("localURL");
                    img.source = table.Rows[i].Field<string>("source");
                    img.tag = table.Rows[i].Field<string>("tag");
                    img.title = table.Rows[i].Field<string>("title");
                    img.dateHit = table.Rows[i].Field<string>("dateHit");
                    img.description = table.Rows[i].Field<string>("description");
                    img.tags = table.Rows[i].Field<string>("tags");
                    img.owner = table.Rows[i].Field<string>("owner");
                    img.origin = table.Rows[i].Field<string>("origin");
                    img.dateTaken = table.Rows[i].Field<string>("dateTaken");
                    img.webImageBase64Encoded = table.Rows[i].Field<string>("webImageBase64Encoded");
                    imagelist.Add(img);
                }
            }
            else
            {
                string criteria = "";
                if (searchtag != null)
                {
                    criteria = "_search?tag=" + searchtag.ToLower();
                }
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/" + criteria);
                var config = new Elasticsearch.Net.ConnectionConfiguration(node).PrettyJson(true);
                var client = new ElasticLowLevelClient(config);
                var query = new object();
                if (searchtag == null)
                {
                    query = new { query = new { term = new { origin = "flickr.com" } } };
                }


                dynamic result = client.Search<Object>(query);


                if (result.Body != null)
                {
                    image img = new image();
                    for (int i = 0; i < result.Body.hits.total; i++)
                    {
                        try
                        {
                            img.imgGUID = result.Body.hits.hits[i]._source.imgGUID;
                            img.title = result.Body.hits.hits[i]._source.title;
                            img.tag = result.Body.hits.hits[i]._source.tag;
                            img.source = result.Body.hits.hits[i]._source.source;
                            img.localURL = result.Body.hits.hits[i]._source.localURL;
                            img.sourceURL = result.Body.hits.hits[i]._source.sourceURL;
                            img.keyword = result.Body.hits.hits[i]._source.keyword;
                            img.origin = result.Body.hits.hits[i]._source.origin;
                            img.owner = result.Body.hits.hits[i]._source.owner;
                            img.tags = result.Body.hits.hits[i]._source.tags;
                            img.dateHit = result.Body.hits.hits[i]._source.dateHit;
                            img.dateTaken = result.Body.hits.hits[i]._source.dateTaken;
                            img.description = result.Body.hits.hits[i]._source.description;
                            img.webImageBase64Encoded = result.Body.hits.hits[i]._source.webImageBase64Encoded;
                            imagelist.Add(img);
                        }
                        catch (Exception)
                        {


                        }

                        img = new image();
                    }
                }
            }
            return imagelist;

        }

        public image SearchImage(string imgGUID)
        {
            image img = new image();
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table = ReadDataSet("SELECT * FROM imagelib with (nolock) where imgGUID='" + imgGUID + "'").Tables[0];
               
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    img = new image();
                    img.imgGUID = table.Rows[i].Field<string>("imgGUID");
                    img.keyword = table.Rows[i].Field<string>("keyword");
                    img.localURL = table.Rows[i].Field<string>("localURL");
                    img.source = table.Rows[i].Field<string>("source");
                    img.tag = table.Rows[i].Field<string>("tag");
                    img.title = table.Rows[i].Field<string>("title");
                    img.dateHit = table.Rows[i].Field<string>("dateHit");
                    img.description = table.Rows[i].Field<string>("description");
                    img.tags = table.Rows[i].Field<string>("tags");
                    img.owner = table.Rows[i].Field<string>("owner");
                    img.origin = table.Rows[i].Field<string>("origin");
                    img.dateTaken = table.Rows[i].Field<string>("dateTaken");
                    img.webImageBase64Encoded = table.Rows[i].Field<string>("webImageBase64Encoded");
                
                }
            }
            else
            {
                string criteria = "";
                if (imgGUID != null)
                {
                    criteria = "_search?imgGUID=" + imgGUID;
                }
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/" + criteria);
                var config = new Elasticsearch.Net.ConnectionConfiguration(node).PrettyJson(true);
                var client = new ElasticLowLevelClient(config);
                var query = new object();
                if (imgGUID == null)
                {
                    query = new { query = new { term = new { origin = "flickr.com" } } };
                }


                dynamic result = client.Search<Object>(query);


                if (result.Body != null)
                {
                    
                    for (int i = 0; i < result.Body.hits.total; i++)
                    {
                        try
                        {
                            img.imgGUID = result.Body.hits.hits[i]._source.imgGUID;
                            img.title = result.Body.hits.hits[i]._source.title;
                            img.tag = result.Body.hits.hits[i]._source.tag;
                            img.source = result.Body.hits.hits[i]._source.source;
                            img.localURL = result.Body.hits.hits[i]._source.localURL;
                            img.sourceURL = result.Body.hits.hits[i]._source.sourceURL;
                            img.keyword = result.Body.hits.hits[i]._source.keyword;
                            img.origin = result.Body.hits.hits[i]._source.origin;
                            img.owner = result.Body.hits.hits[i]._source.owner;
                            img.tags = result.Body.hits.hits[i]._source.tags;
                            img.dateHit = result.Body.hits.hits[i]._source.dateHit;
                            img.dateTaken = result.Body.hits.hits[i]._source.dateTaken;
                            img.description = result.Body.hits.hits[i]._source.description;
                            img.webImageBase64Encoded = result.Body.hits.hits[i]._source.webImageBase64Encoded;
                           
                        }
                        catch (Exception)
                        {


                        }

                        img = new image();
                    }
                }
            }
            return img;

        }

        public string ImageToBase64(Image image,
                System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        public Byte[] Base64ToByte(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);
                    
            return imageBytes;
        }
    }
}
