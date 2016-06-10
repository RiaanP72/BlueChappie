using System;
using System.Collections.Generic;
using System.Web;
using Elasticsearch.Net;
namespace BlueChappie.Models
{
    public class MainApplicationModels 
    {
        public class image {
            private string _guid;
            public string imgGUID {
                get {
                    if (_guid == null)
                    {
                        Guid _g = Guid.NewGuid();
                        string _s = _g.ToString();
                        _guid = _s;
                        return _s;
                        }
                    else {
                        return _guid;
                    }
                }
                set { _guid = imgGUID; }
            }
            public string title { get; set; }
            public string source { get; set; }
            public string localURL { get; set; }
            public string sourceURL { get; set; }
            public string keywords { get; set; }
        }
        public class images<image>:List<image> {
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
