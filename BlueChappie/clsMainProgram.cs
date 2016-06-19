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
        //
        //    ReadDataSet is used to read information from the SQL Server. I prefer using one central functions and to limit points of failure.
        //    It uses the standard System.Data classes by Microsoft. 
         
        public System.Data.DataSet ReadDataSet(String SQLCommand)
        {
            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["SQLConnectionString"]);
            System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.DataSet dataSet = new System.Data.DataSet();
            sqlDataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand(SQLCommand, sqlConnection);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            sqlDataAdapter.Fill(dataSet, "SQL Data Set");
            return dataSet;
        }

        //
        // RunSQL is used to issue instructions to the DataBase for inserting or deleteing information
        public void RunSQL(String SQLCommand)
        {
            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["SQLConnectionString"]);
            sqlConnection.Open();
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(SQLCommand, sqlConnection);
            sqlCommand.ExecuteNonQuery();
        }
        //
        // Normal JSON conversion using Newtonsoft
        //
        public string tbl2JSON(DataTable dataTable)
        {
            string JSONresult;
            JSONresult = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
            return JSONresult;
        }
        //
        // XML conversion using a memory stream.
        // I prefer utilising system memory and CPU and not creating unnecessary disk IO
        //
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
            // API call derived from: https://www.flickr.com/servicesapi/explore/flickr.photos.search
            //
            //  Important site to know about: http://www.programmableweb.com/
            //
            // the Flickr API key : 12ba11510a21ba897117735df462cdca and secret (12d265d5e9d5d421) for this application

            string _response = readAPI("https://api.flickr.com/services/rest/?method=flickr.photos.search&tags=" + tag + "&text=" + keyword + "&safe_search=&format=rest&api_key=12ba11510a21ba897117735df462cdca&extras=description, date_upload, date_taken, owner_name, tags, description", tag, keyword);
            return _response;
        }
        //
        //  readAPI reads the flickr.com web API's. This part should have been a background task, but not enough time at the moment
        //
        // TODO: READAPI, Make backgroud task
        //
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
                            
                                _guid = Guid.NewGuid();
                                imageinfo.imgGUID = _guid.ToString();
                            //
                            //  I stoped checking the description field as it keeps returing NULL, but can be firgured out.
                            //
                               // imageinfo.description = xmlreader.GetAttribute("description").Replace("'","`");
                                imageinfo.title = xmlreader.GetAttribute("title").Replace("'", "`");
                                imageinfo.localURL = imagepth;
                                imageinfo.sourceURL = imagepth;
                                imageinfo.source = apiURL;
                                imageinfo.tag = tag;
                                imageinfo.keyword = keyword;
                                imageinfo.dateHit = DateTime.Now.ToString().Replace("/","-");
                                imageinfo.dateTaken = xmlreader.GetAttribute("datetaken");
                                imageinfo.tags = xmlreader.GetAttribute("tags").Replace("'", "`");
                                imageinfo.owner = xmlreader.GetAttribute("ownername").Replace("'", "`");
                                imageinfo.origin = "flickr.com";
                                imageinfo.sourceID = xmlreader.GetAttribute("id");
                                imageinfo.sourceSecret = xmlreader.GetAttribute("secret");
                                imageinfo.sourceFarm = xmlreader.GetAttribute("farm");
                                imageinfo.sourceServer= imageinfo.sourceID = xmlreader.GetAttribute("server");
                            if (!CheckExsitingImageSourceURL(imageinfo))
                            {
                                request = WebRequest.Create(imageinfo.sourceURL);
                                imgStream = request.GetResponse();
                                try
                                {
                                    imageinfo.webImageBase64Encoded = ImageToBase64(Image.FromStream(imgStream.GetResponseStream()), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                catch (Exception)
                                {
                                    break;    
                                }
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
        //
        // getLocationsWithThumbNails reads from the data system to generate a filled class with location info and thumbnails for
        //  the images associated with the location
        //

        public locationsWithThumbNails<locationWithThumbNails> getLocationsWithThumbNails(string userID="_all")
        {
            locationsWithThumbNails<locationWithThumbNails> locationlist = new locationsWithThumbNails<locationWithThumbNails>();
            locationWithThumbNails locnt = new locationWithThumbNails();
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table = ReadDataSet("SELECT tag, LOWER(REPLACE(RTRIM(tag), ' ', '')) AS tKey, COUNT(*) AS cnt FROM dbo.imagelib WITH (nolock) GROUP BY LOWER(REPLACE(RTRIM(tag), ' ', '')), tag").Tables[0];
                DataTable usertable = ReadDataSet("SELECT tagKey FROM userfavlocationslib with (nolock) WHERE userid='" + userID + "'").Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    locnt = new locationWithThumbNails();
                    locnt.tag = table.Rows[i].Field<string>("tag");
                    locnt.counter = table.Rows[i].Field<int>("cnt");
                    locnt.locationimages = SearchImagesPerLocation(table.Rows[i].Field<string>("tKey"));
                    for (int j = 0;j< usertable.Rows.Count; j++)
                    {
                        if (usertable.Rows[j].Field<string>("tagKey").Equals(table.Rows[i].Field<string>("tKey"))) {
                            locnt.isFavourite = true;
                            break;
                        }
                    }
                    locationlist.Add(locnt);
                }
            }
            else
            {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/");
                var client = new Nest.ElasticClient(node);
                var query = client.Search<image>((s => s
                        .Aggregations(a => a
                            .Terms("group_by_tag", ts => ts
                                .Field(o => o.tag)
                                .Aggregations(aa => aa
                                    .ValueCount("valuecount_imgGUID", sa => sa
                                        .Field(o => o.imgGUID)
                                    )
                                )
                            )
                        ))
                    );
                var terms = query.Aggs.Terms("group_by_tag");
                try
                {
                    foreach (var tag in terms.Buckets)
                    {
                        locnt.counter = Int16.Parse(tag.DocCount.Value.ToString());
                        locnt.tag = tag.Key;

                        locnt.locationimages = SearchImagesPerLocation(locnt.tKey);
                        locationlist.Add(locnt);


                        locnt = new locationWithThumbNails();
                    };
                }
                catch (Exception)
                {


                }

            }

            return locationlist;
        }
        //
        // getLocationsWithThumbNails reads from the data system to generate a filled class with location info WITHOUT the 
        // thumbnails for the images associated with the location
        //
        public locations<location> getLocations()
        {
            locations<location> locationlist = new locations<location>();
            location locnt = new location();
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table = ReadDataSet("SELECT tag, LOWER(REPLACE(RTRIM(tag), ' ', '')) AS tKey, COUNT(*) AS cnt FROM dbo.imagelib WITH (nolock) GROUP BY LOWER(REPLACE(RTRIM(tag), ' ', '')), tag").Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    locnt = new location();
                    locnt.tag = table.Rows[i].Field<string>("tag");
                    locnt.counter = table.Rows[i].Field<int>("cnt");
                    locationlist.Add(locnt);
                }
            }
            else
            {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/");
                var client = new Nest.ElasticClient(node);
                var query = client.Search<image>((s => s
                        .Aggregations(a => a
                            .Terms("group_by_tag", ts => ts
                                .Field(o => o.tag)
                                .Aggregations(aa => aa
                                    .ValueCount("valuecount_imgGUID", sa => sa
                                        .Field(o => o.imgGUID)
                                    )
                                )
                            )
                        ))
                    );
                var terms = query.Aggs.Terms("group_by_tag");
                try
                {
                    foreach (var tag in terms.Buckets)
                    {
                        locnt.counter = Int16.Parse(tag.DocCount.Value.ToString());
                        locnt.tag = tag.Key;
                        locationlist.Add(locnt);


                        locnt = new location();
                    };
                }
                catch (Exception)
                {


                }

            }

            return locationlist;
        }
        //
        //  saveLocation keeps the location datasysem up to date whenever images are retrieved online
        //
        //
        public void saveLocation(String location)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                string strSQL = "IF (SELECT count(*) FROM locationlib with (nolock) where location='" + location.ToLower() + "')=0  INSERT INTO locationlib(location,created) VALUES(";
                strSQL += "'" + location.ToLower() + "',";
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
        //
        // SaveImage creates a UNIQUE record of images stored in the data system, including the image METADATA
        //
        public void SaveImage(image img)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                string strSQL = "IF (SELECT count(*) FROM imagelib with (nolock) where sourceURL='" + img.sourceURL + "')=0  INSERT INTO imagelib(imgGUID,keyword,localURL,source,sourceURL,tag,title,dateHit,description,tags,owner,origin,dateTaken,tagKey,sourceServer,sourceID,sourceSecret,sourceFarm,webImageBase64Encoded) VALUES(";
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
                strSQL += "'" + img.tagKey + "',";
                strSQL += "'" + img.sourceServer + "',";
                strSQL += "'" + img.sourceID + "',";
                strSQL += "'" + img.sourceSecret + "',";
                strSQL += "'" + img.sourceFarm + "',";
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
        //
        // CheckExsitingImageSourceURL is used to check the data system for the existence of an online found image to prevent duplication
        //
        public bool CheckExsitingImageSourceURL(image img)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                if (ReadDataSet("SELECT count(*) FROM imagelib with (nolock) where sourceURL='" + img.sourceURL + "'").Tables[0].Rows[0].Field<int>(0) == 0)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }
            else {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/");
                var client = new Nest.ElasticClient(node);
                Boolean _found = false;
                var query = client.Search<image>(s => s
                          
                          
                          .Query(q => q
                          .ConstantScore(c=>c
                          .Filter(fl=>fl
                                .Term(t => t
                                .Field(f => f.sourceID)
                                .Value(img.sourceID)
                                .Field(f => f.sourceSecret)
                                .Value(img.sourceSecret)
                                .Field(f => f.sourceServer)
                                .Value(img.sourceServer)
                                .Field(f => f.sourceFarm)
                                .Value(img.sourceFarm)
                                )
                                )
                                )
                                )
                            );
                var docs = query.Documents;
                foreach (var tag in docs)
                {

                        _found = true;
                }
                return _found;

            }
        }
        //
        //  SearchImagesPerLocation retrieves a list of images for a supplied location from the data system
        //
        public images<image> SearchImagesPerLocation(string searchtag )
        {
            images<image> imagelist = new images<image>();
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table;
                if (searchtag == null || searchtag.Equals("") || searchtag.Equals("_all"))
                {
                    table = ReadDataSet("SELECT * FROM imagelib with (nolock)").Tables[0];
                    
                }
                else {
                    table = ReadDataSet("SELECT * FROM imagelib with (nolock) where tag='" + searchtag + "' or LOWER(REPLACE(RTRIM(tag), ' ', ''))='" + searchtag + "'").Tables[0];
                }
                
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
                    criteria = "_search?tag=" + searchtag;
                }
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/" + criteria);
                var config = new Elasticsearch.Net.ConnectionConfiguration(node);
                var client = new ElasticLowLevelClient(config);
                var query = new object();
                query = new { query = new { term = new { tag = searchtag } } };
                if (searchtag == null || searchtag.Equals("")|| searchtag.Equals("_all"))
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
                            break;
                        }

                        img = new image();
                    }
                }
            }
            return imagelist;
        }
        public image SearchImage(string searchGUID)
        {
            image img = new image();
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table = ReadDataSet("SELECT * FROM imagelib with (nolock) where imgGUID='" + searchGUID + "'").Tables[0];
               
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

                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/imagelib/");
                var client = new Nest.ElasticClient(node);
                var query = client.Search<image>(s => s

                         .Query(q => q.Bool(b => b.Must(
                                m => m.MatchAll(),
                                m => m.Term(t => t.Field(f => f.imgGUID).Value(searchGUID)))))



                    );
                var docs = query.Documents;
                foreach (var tag in docs)
                {

                    img.imgGUID = tag.imgGUID;
                    img.title = tag.title;
                    img.tag = tag.tag;
                    img.source = tag.source;
                    img.localURL = tag.localURL;
                    img.sourceURL = tag.sourceURL;
                    img.keyword = tag.keyword;
                    img.origin = tag.origin;
                    img.owner = tag.owner;
                    img.tags = tag.tags;
                    img.dateHit = tag.dateHit;
                    img.dateTaken = tag.dateTaken;
                    img.description = tag.description;
                    img.webImageBase64Encoded = tag.webImageBase64Encoded;
                                       
                };
                
            }
            return img;

        }
        public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
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
        //
        //  getUserDetails searches the data system for the details of a loggin in user 
        //
        public user getUserDetails(string userId) {
            user usr = new user();
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table = ReadDataSet("SELECT * FROM userlib with (nolock) where userId='" + userId + "'").Tables[0];


                for (int i = 0; i < table.Rows.Count; i++)
                {
                        usr.userId = table.Rows[i].Field<string>("userId");
                        usr.emailaddress = table.Rows[i].Field<string>("emailaddress");
                        usr.password = table.Rows[i].Field<string>("password");
                }
            }
            else
            {

                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/userlib/");
                var client = new Nest.ElasticClient(node);
                var query = client.Search<user>(s => s

                     .Query(q => q.ConstantScore(c => c
                                .Filter(l => l
                                .Term(t => t
                                .Field(f => f.userId)
                                .Value(userId)
                                ))))
                    );
                var docs = query.Documents;
                foreach (var doc in docs)
                {
                    usr.userId = doc.userId;
                    usr.emailaddress = doc.emailaddress;
                    usr.password = doc.password;
                }
            }
            return usr;
        }
        //
        //  login, well it tests the supplied login information and if the user does not exist it is autmaticaly created
        //
        public user login(string emailaddress, string password)
        {
            user usr = new user();
            Boolean _exists = false;
            Boolean _success = false;

            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
            {
                DataTable table = ReadDataSet("SELECT * FROM userlib with (nolock) where emailaddress='" + emailaddress + "'").Tables[0];


                for (int i = 0; i < table.Rows.Count; i++)
                  {
                    _exists = true;

                    if (password.Equals(table.Rows[i].Field<string>("password")))
                    {
                        usr.userId = table.Rows[i].Field<string>("userId");
                        usr.emailaddress = emailaddress;
                        usr.password = password;
                        _success = true; 
                    }
                    
                }

                if (!_exists)
                {
                    usr.emailaddress = emailaddress;
                    usr.password = password;
                    Guid guid = new Guid();
                    usr.userId = guid.ToString();
                    string strSQL = "IF (SELECT count(*) FROM userlib with (nolock) where emailaddress='" + usr.emailaddress + "')=0  INSERT INTO userlib(userid,emailaddress,password) VALUES(";
                    strSQL += "'" + usr.userId + "',";
                    strSQL += "'" + usr.emailaddress + "',";
                    strSQL += "'" + usr.password + "')";
                    RunSQL(strSQL);
                    _success = true;
                }
            }
            else
            {

                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/userlib/");
                var client = new Nest.ElasticClient(node);
                var query = client.Search<user>(s => s

                     .Query(q => q.ConstantScore(c=>c
                                .Filter(l=>l
                                .Term(t => t
                                .Field(f => f.emailloggin)
                                .Value(emailaddress.Replace("@","."))
                                ))))
                    );
                var docs = query.Documents;
                foreach (var doc in docs)
                {
                    _exists = true;


                    if (password.Equals(doc.password))
                    {
                       
                        usr.userId = doc.userId;
                        usr.emailaddress = doc.emailaddress;
                        usr.password = doc.password;
                        _success = true;   
                    }
                    

                };
                if (!_exists)
                {
                    usr.emailaddress = emailaddress;
                    usr.password = password;
                    Guid guid = Guid.NewGuid();
                    usr.userId = guid.ToString();
                    var saveconfig = new ConnectionConfiguration(node);
                    var saveclient = new ElasticLowLevelClient(saveconfig);
                    string usrInfo = Newtonsoft.Json.JsonConvert.SerializeObject(usr);
                    PostData<object> postingData = usrInfo;
                    dynamic result0 = saveclient.DoRequest<object>(HttpMethod.PUT, usr.userId, postingData);
                    _success = true;
                }

            }

            if (!_success) { usr.userId = "z"; }

            return usr;

        }
        //
        // setFavouriteLocation checks and switches the logged in user's favourite selections
        //
        public Boolean setFavouriteLocation(string userID, String LocationTagKey) {
            Boolean _isFav = false;
            Guid _guid = Guid.NewGuid();
            if (userID == null || userID.Equals("") || LocationTagKey == null || LocationTagKey.Equals(""))
            {
                _isFav = false;
            }
            if (System.Configuration.ConfigurationManager.AppSettings["StorageType"].Equals("SQL"))
                {
                    if (ReadDataSet("SELECT count(*) as CountCheck FROM userfavlocationslib with (nolock) where userid='" + userID + "' and tagKey='" + LocationTagKey + "'").Tables[0].Rows[0].Field<int>("CountCheck") == 0)
                    {
                        _isFav = true;
                        userFavLocation favloc = new userFavLocation();
                        favloc.tkey = LocationTagKey;
                        favloc.userid = userID;
                        favloc.userLocationId = _guid.ToString();
                        RunSQL("INSERT INTO userfavlocationslib (userlocationid,userid,tagKey) VALUES ('" + favloc.userLocationId + "','" + favloc.userid + "','" + favloc.tkey + "')");
                    }
                    else {
                        _isFav = false;
                        RunSQL("DELETE FROM userfavlocationslib WHERE userid='" + userID + "' AND tagKey='" + LocationTagKey + "'");
                    }
                }
                
            else
            {
                var node = new Uri(blueChappieSetttings.BlueChappieElasticServer + "/userfavlocationslib/");
                var client = new Nest.ElasticClient(node);
                userFavLocation userfavlocation = new userFavLocation();
                var query = client.Search<userFavLocation>(s => s


                          .Query(q => q
                          .ConstantScore(c => c
                          .Filter(fl => fl
                                .Term(t => t
                                .Field(f => f.userid)
                                .Value(userID)
                                .Field(f => f.tkey)
                                .Value(LocationTagKey)
                                )
                                )
                                )
                                )
                            );
                var docs = query.Documents;

                foreach (var doc in docs) {
                    _isFav = true;
                    userfavlocation.userid = doc.userid;
                    userfavlocation.tkey = doc.tkey;
                    userfavlocation.userLocationId = doc.userLocationId;
                }
                if (_isFav)
                {
                    var config = new ConnectionConfiguration(node);
                    var lowlevelclient = new ElasticLowLevelClient(config);
                    string localtionInfo = Newtonsoft.Json.JsonConvert.SerializeObject(userfavlocation);
                    PostData<object> postingData = localtionInfo;
                    dynamic result0 = lowlevelclient.DoRequest<object>(HttpMethod.DELETE, userfavlocation.userLocationId, postingData);
                    _isFav = false;
                }
                else {
                    
                    var config = new ConnectionConfiguration(node);
                    var lowlevelclient = new ElasticLowLevelClient(config);
                    userfavlocation.userid = userID;
                    userfavlocation.tkey = LocationTagKey;
                    userfavlocation.userLocationId = _guid.ToString();
                    string localtionInfo = Newtonsoft.Json.JsonConvert.SerializeObject(userfavlocation);
                    PostData<object> postingData = localtionInfo;
                    dynamic result0 = lowlevelclient.DoRequest<object>(HttpMethod.PUT, userfavlocation.userLocationId, postingData);
                    _isFav = true;
                }

            }

            return _isFav;
        }
    }
}
