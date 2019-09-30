using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Infrastructure.Interfaces
{
    public interface IStopInfra
    {
        Task<HttpResponseMessage> GetStops(string lattiude, string longitude, string data, string hora);
    }
}
