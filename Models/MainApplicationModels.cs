using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;

namespace BlueChappie.Models
{
    public class BlueChappieModels
    {
        public class BlueChappieID  {
            
            public string NewID {get{
                    Guid _g = Guid.NewGuid();
                    return _g.ToString().Replace("-","");
                }}
            

        }
        
        public class BlueChappieSettings
        {
            private string _ElasticSearchServer;
            private string _ElasticSearchServerPort;
            private string _ElasticSearchServerIndex;
            private string _BlueChappieElasticServer;
            public string ElasticSearchServer
            {
                get
                {
                    if (_ElasticSearchServer == null)
                    {
                        _ElasticSearchServer = System.Configuration.ConfigurationManager.AppSettings["ElasticSearchServer"];
                        return _ElasticSearchServer;
                    }
                    else { return _ElasticSearchServer; }
                }
                set { _ElasticSearchServer = ElasticSearchServer; }
            }

            public string ElasticSearchServerPort

            {
                get
                {
                    if (_ElasticSearchServerPort == null)
                    {
                        _ElasticSearchServerPort = System.Configuration.ConfigurationManager.AppSettings["ElasticSearchServerPort"];
                        return _ElasticSearchServerPort;
                    }
                    else { return _ElasticSearchServerPort; }
                }
                set { _ElasticSearchServerPort = ElasticSearchServerPort; }
            }

            public string ElasticSearchServerIndex

            {
                get
                {
                    if (_ElasticSearchServerIndex == null)
                    {
                        _ElasticSearchServerIndex = System.Configuration.ConfigurationManager.AppSettings["ElasticSearchServerIndex"];
                        return _ElasticSearchServerIndex;
                    }
                    else { return _ElasticSearchServerIndex; }
                }
                set { _ElasticSearchServerIndex = ElasticSearchServerIndex; }
            }
            public string BlueChappieElasticServer
                
            {
                get
                {
                    if (_BlueChappieElasticServer == null)
                    {
                        _BlueChappieElasticServer = ElasticSearchServer + ":" + ElasticSearchServerPort + "/" + ElasticSearchServerIndex;
                        return _BlueChappieElasticServer;
                    }
                    else { return _BlueChappieElasticServer; }
                }
            }
        }
        public class scanstatus {
            public string ClientRequestId { get; set; }
            public string ClientRequestIdStatus { get; set; }
        }
        public class user {
            public string emailaddress { get; set; }
            public string emailloggin { get {
                    if (emailaddress == null)
                    {
                        return "";
                    }
                    else
                    {
                        return emailaddress.Replace("@", ".");
                    }
                } }
            public string password { get; set; }
            private string _guidUser;
            public string userId
            {
                get { return _guidUser.Replace("-", ""); }
                set { this._guidUser = value; }
            }

        }
  
        public class userFavLocation {
            public string tkey { get; set; }
            public string userid { get; set; }
            public Boolean isFavourite { get; set; }
            private string _guidLocation;
            public string userLocationId
            {
                get { return _guidLocation.Replace("-", ""); }
                set { this._guidLocation = value; }
            }

        }
        public class userFavLocations<userFavLocation> : List<userFavLocation>, IEnumerable
        {
            public userFavLocations() : base() { }
            public userFavLocations(userFavLocation item)
            { Add(item); }
            public userFavLocations(userFavLocation[] items)
            {
                foreach (userFavLocation item in items)
                { Add(item); }
            }
        }
        public class userLikedImage
        {
            public string tkey { get; set; }
            public string userid { get; set; }
            private string _guidUserLikedImage;
            public string userLikedImageId
            {
                get { return _guidUserLikedImage.Replace("-", ""); }
                set { this._guidUserLikedImage = value; }
            }

        }
        public class userLikedImages<userLikedImage> : List<userLikedImage>, IEnumerable
        {
            public userLikedImages() : base() { }
            public userLikedImages(userLikedImage item)
            { Add(item); }
            public userLikedImages(userLikedImage[] items)
            {
                foreach (userLikedImage item in items)
                { Add(item); }
            }
        }
        public class location {
            public string tKey { get
                {
                    return tag.Replace(" ", "").ToLower();
                  }      
                      }
            public string tag { get; set; }
            public int counter { get; set; }
            public Boolean isFavourite { get; set; }
            


        }
        public class locationWithThumbNails
        {
            public string tKey
            {
                get
                {
                    return (tag == null) ? "" : tag.Replace(" ", "").ToLower();
                }
            }
            public string tag { get; set; }
            public int counter { get; set; }
            public Boolean isFavourite { get; set; }
            public images<image> locationimages { get; set; }


        }
        public class locations<location> : List<location>, IEnumerable
        {
            public locations() : base() { }
            public locations(location item)
            { Add(item); }
            public locations(location[] items)
            {
                foreach (location item in items)
                { Add(item); }
            }
        }
        public class locationsWithThumbNails<locationWithThumbNails> : List<locationWithThumbNails>, IEnumerable
        {
            public locationsWithThumbNails() : base() { }
            public locationsWithThumbNails(locationWithThumbNails item)
            { Add(item); }
            public locationsWithThumbNails(locationWithThumbNails[] items)
            {
                foreach (locationWithThumbNails item in items)
                { Add(item); }
            }
        }

        public class image {
            private string _guidImage;
            private BlueChappieID bcid = new BlueChappieID();
            public string imgGUID {
                get { return (_guidImage == null) ? bcid.NewID : _guidImage.Replace("-", ""); }
                set { this._guidImage = value; } }
            public string title { get; set; }
            public string description { get; set; }
            public string tag { get; set; }
            public string tags { get; set; }
            public string source { get; set; }
            public string localURL { get; set; }
            public string sourceURL { get; set; }
            public string sourceServer { get; set; }
            public string sourceID { get; set; }
            public string sourceSecret { get; set; }
            public string sourceFarm { get; set; }
            public string keyword { get; set; }
            public string owner { get; set; }
            public string origin { get; set; }
            public string dateHit { get; set; }
            public string dateTaken { get; set; }
            public string webImageBase64Encoded { get; set; }
            public string webImageThumbnail240x250Base64Encoded
            {
                get
                {
                    clsMainProgram cls = new clsMainProgram();
                    Image img = cls.Base64ToImage(webImageBase64Encoded).GetThumbnailImage(247, 250, () => false, IntPtr.Zero);
                    return cls.ImageToBase64(img, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            public string webImageThumbnailBase64Encoded { get
                {
                    clsMainProgram cls = new clsMainProgram();
                    Image img = cls.Base64ToImage(webImageBase64Encoded).GetThumbnailImage(50, 50, () => false, IntPtr.Zero);
                    return cls.ImageToBase64(img,System.Drawing.Imaging.ImageFormat.Png);
                }
                     }
            public string tagKey { get
                {
                    return this.tag.Replace(" ", "").ToLower();

                }
            }

            public image(image listitems)
            {
                this.imgGUID = listitems.imgGUID;
                this.keyword = listitems.keyword;
                this.localURL = listitems.localURL;
                this.source = listitems.source;
                this.sourceURL = listitems.sourceURL;
                this.tag = listitems.tag;
                this.title = listitems.title;
                this.dateHit = listitems.dateHit;
                this.description = listitems.description;
                this.tags = listitems.tags;
                this.owner = listitems.owner;
                this.origin = listitems.origin;
                this.dateTaken = listitems.dateTaken;
                this.webImageBase64Encoded = listitems.webImageBase64Encoded;
                this.sourceFarm = listitems.sourceFarm;
                this.sourceID = listitems.sourceID;
                this.sourceSecret = listitems.sourceSecret;
                this.sourceServer = sourceServer;
            }
            public image()
            {
                //
            }
        }
        public class images<image> : List<image>,IEnumerable
        {
            public images() : base() { }
            public images(image item)
            { Add(item); }
            public images(image[] items)
            {
                foreach (image item in items)
                { Add(item); }
            }
        }
        public class plainList 
        {
            public plainList(string listitem){
            this.plainlistitem = listitem;
            }
            public string plainlistitem { get; set; }
            
        }
        public class PlainList<plainlist> : List<plainlist> {
            public PlainList() : base() { }
            public PlainList(plainlist item)
            { Add(item); }
            public PlainList(plainlist[] items)
            { foreach (plainlist item in items)
                { Add(item); } }
        }
    }
}
