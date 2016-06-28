using BlueChappie.Hubs;
using Microsoft.AspNet.SignalR;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Services
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StatisticsService
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }

        public async Task NotifyUpdates(string ClientRequestId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<StatisticsHub>();
            if (hubContext != null)
            {
                var stats = await this.GenerateStatisticsAsync(ClientRequestId);
                hubContext.Clients.All.updateStatistics(stats);
            }
        }
        public async Task<string> GenerateStatisticsAsync(string ClientRequestId)
        {
            clsMainProgram cls = new clsMainProgram();
            //await GenerateStatistics.Delay
            scanstatus scnstat = new scanstatus();
            string status = await cls.GetStatusAsync(ClientRequestId);
            return status;
            //scnstat.ClientRequestIdStatus;
            //return Task.Run(() =>cls.GetStatusAsync(ClientRequestId)).GetAwaiter().GetResult();
            }
        // Add more operations here and mark them with [OperationContract]
    }
}
