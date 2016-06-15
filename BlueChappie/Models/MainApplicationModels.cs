using System;
using System.Collections.Generic;
using System.Web;
using Elasticsearch.Net;
using System.Collections;
using System.Drawing;

namespace BlueChappie.Models
{
    public class BlueChappieModels 
    {
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
                        _ElasticSearchServer = "http://10.0.0.10";
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
                        _ElasticSearchServerPort = "9200";
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
                        _ElasticSearchServerIndex = "bluechappie";
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
        public class image {
            public string imgGUID { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string tag { get; set; }
            public string tags { get; set; }
            public string source { get; set; }
            public string localURL { get; set; }
            public string sourceURL { get; set; }
            public string keyword { get; set; }
            public string owner { get; set; }
            public string origin { get; set; }
            public string dateHit { get; set; }
            public string dateTaken { get; set; }
            public string webImageBase64Encoded { get; set; }
                    
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
