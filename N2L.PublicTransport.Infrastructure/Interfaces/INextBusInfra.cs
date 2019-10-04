using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Infrastructure.Interfaces
{
    public interface INextBusInfra
    {
        List<HttpResponseMessage> GetNextBus(string startLatitude, string startLongitude, string[] destinations, string dateTime);
        Task<HttpResponseMessage> GetRoutes(string startLatitude, string startLongitude, string endLatitude, string endLongitude, string data, string hora, bool isArrivalTime);
    }
}
