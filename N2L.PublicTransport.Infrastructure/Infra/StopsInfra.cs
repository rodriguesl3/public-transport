using Flurl.Http;
using N2L.PublicTransport.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Infrastructure.Infra
{
    public class StopsInfra : IStopInfra
    {
        public async Task<HttpResponseMessage> GetStops(string latitude, string longitude, string data, string hora)
        {
            var response = await Environment.GetEnvironmentVariable("LISBON_SEARCH_STOP")
                 .PostMultipartAsync(mp =>
                 mp.AddString("cmd", "pesquisarParagem")
                   .AddString("coordenadas", $"{latitude}/{longitude}")
                   .AddString("hora", $"{hora}")
                   .AddString("data", $"{data}")
                   .AddString("UrlBase", Environment.GetEnvironmentVariable("LISBON_DOMAIN"))
                   .AddString("areaInfluencia", "500")
                   .AddString("intervalo", "1000")
                   .AddString("textLocal", "Definido no mapa")
                   .AddString("codOperador", "")
                 );
            return response;
        }
    }
}
