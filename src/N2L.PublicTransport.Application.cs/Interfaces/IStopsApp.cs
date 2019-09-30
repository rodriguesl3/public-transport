using N2L.PublicTransport.Domain.Aggregation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Application.Interfaces
{
    public interface IStopsApp
    {
        Task<IEnumerable<StopLocation>> GetStopsLocations(string latitude, string longitude, string data, string hora);
    }
}
