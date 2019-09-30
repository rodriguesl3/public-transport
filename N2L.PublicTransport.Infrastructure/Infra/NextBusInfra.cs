using Flurl.Http;
using N2L.PublicTransport.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Infrastructure.Infra
{
    public class NextBusInfra : INextBusInfra
    {
        public List<HttpResponseMessage> GetNextBus(string startLatitude, string startLongitude, string[] destinations)
        {
            var taskList = new List<Task>();
            var httpResponseList = new List<HttpResponseMessage>();

            foreach (var destination in destinations)
            {
                var splitLocation = destination.Split(',');
                string endLatitude = splitLocation[0];
                string endLongitude = splitLocation[1];


                var task = Environment.GetEnvironmentVariable("LISBON_SEARCH_ROUTE")
                .PostMultipartAsync(mp =>
                 mp.AddString("textPartida", "Definido no mapa")
                    .AddString("latInicial", startLatitude)
                    .AddString("longInicial", startLongitude)

                    .AddString("textChegada", "Definido no mapa")
                    .AddString("latFinal", endLatitude)
                    .AddString("longFinal", endLongitude)

                    .AddString("dataRota", DateTime.Now.ToString("dd/MM/yyyy"))
                    .AddString("horaRota", DateTime.Now.ToString("hh:mm"))
                    .AddString("calculoPartida", "0")
                    .AddString("calculoPreferencial", "false")
                    .AddString("nAlt", "1")
                    .AddString("meiosTransporte", "true,true,true,true")
                    .AddString("tipoPercurso", "1") //Mais rapido, Menos Pedonal, Menos Transbordos
                    .AddString("avoidAffectedLines", "False")
                    .AddString("language", "pt-PT")
                    .AddString("cmd", "recuperarRota")
                    .AddString("UrlBase", Environment.GetEnvironmentVariable("LISBON_DOMAIN"))
                    .AddString("isKyosk", "false"))
                .ContinueWith(res => httpResponseList.Add(res.Result));

                taskList.Add(task);
            }

            taskList.ForEach(x => x.Wait());

            return httpResponseList;

        }

        public async Task<HttpResponseMessage> GetRoutes(string startLatitude, string startLongitude, string endLatitude, string endLongitude, string data, string hora, bool isArrivalTime)
        {
            var response = await Environment.GetEnvironmentVariable("LISBON_SEARCH_ROUTE")
                 .PostMultipartAsync(mp =>
                         mp.AddString("textPartida", "Definido no mapa")
                            .AddString("latInicial", startLatitude)
                            .AddString("longInicial", startLongitude)

                            .AddString("textChegada", "Definido no mapa")
                            .AddString("latFinal", endLatitude)
                            .AddString("longFinal", endLongitude)

                            .AddString("dataRota", data)
                            .AddString("horaRota", hora)
                            .AddString("calculoPartida", isArrivalTime ? "1" : "0")
                            .AddString("calculoPreferencial", "false")
                            .AddString("nAlt", "3")
                            .AddString("meiosTransporte", "true,true,true,true")
                            .AddString("tipoPercurso", "1") //Mais rapido, Menos Pedonal, Menos Transbordos
                            .AddString("avoidAffectedLines", "False")
                            .AddString("language", "pt-PT")
                            .AddString("cmd", "recuperarRota")
                            .AddString("UrlBase", Environment.GetEnvironmentVariable("LISBON_DOMAIN"))
                            .AddString("isKyosk", "false")
                    );

            return response;
        }
    }
}
