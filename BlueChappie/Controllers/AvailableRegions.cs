using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Elasticsearch.Net;
namespace BlueChappie.Controllers
{
    public class AvailableRegions : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        
     
    }
        
    
    
}