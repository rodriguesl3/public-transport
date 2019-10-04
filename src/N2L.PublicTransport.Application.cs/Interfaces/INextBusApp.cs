using N2L.PublicTransport.Domain.Aggregation;
using N2L.PublicTransport.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Application.Interfaces
{
    public interface INextBusApp
    {
        Task<List<TravelInformation>> GetTravelInformation(string startLatitude, string startLongitude, string[] destinations, string dateTime);
        Task<List<Route>> GetRoutes(string startLatitude, string startLongitude, string endLatitude, string endLongitude, string data, string hora, bool isArrivalTime);
    }
}
