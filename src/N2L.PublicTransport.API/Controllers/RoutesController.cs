using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using N2L.PublicTransport.Domain.Entities;

using N2L.PublicTransport.CrossCutting.Extensions;
using AutoMapper;
using N2L.PublicTransport.Domain.ViewModel;
using N2L.PublicTransport.Domain.Aggregation;

namespace N2L.PublicTransport.API.Controllers
{
    [Route("api/routes")]
    public class RoutesController : Controller
    {
        private readonly IMapper _mapper;
        public RoutesController(IMapper mapper)
        {
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string startLatitude, string startLongitude, string endLatitude, string endLongitude, string data, string hora, bool isArrivalTime)
        {
            try
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

                var routes = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<RouteViewModel>>(await response.Content.ReadAsStringAsync());

                var routeOptions = new List<Route>();
                int routeIndex = 0;
                foreach (var route in routes)
                {
                    routeIndex++;

                    var routeOption = new Route();

                    var travelInformation = new TravelInformation();

                    var contentSplitted = route.Result.Split(new[] { "#---#" }, StringSplitOptions.RemoveEmptyEntries);
                    var coordinatesSections = contentSplitted[1].Split(new[] { "#-#" }, StringSplitOptions.RemoveEmptyEntries);
                    var sectionInfo = contentSplitted[2].Split(new[] { "#-#" }, StringSplitOptions.RemoveEmptyEntries);

                    var htmlToJson = await contentSplitted[0].ToJsonGenericMode();

                    var htmlResult = JsonConvert.DeserializeObject(await htmlToJson.Content.ReadAsStringAsync());

                    var contentResult = JObject.Parse(await htmlToJson.Content.ReadAsStringAsync());

                    travelInformation = _mapper.Map<TravelInformation>(contentResult);

                    //TODO: Create Mapper.
                    for (int i = 0; i < coordinatesSections.Length; i++)
                    {
                        var sectionList = coordinatesSections[i].Split(new[] { "#*#" }, StringSplitOptions.RemoveEmptyEntries);
                        var pathInfo = sectionInfo[i].Split('|', StringSplitOptions.RemoveEmptyEntries);

                        var coordinates = new List<RouteCoordinates>();

                        foreach (var item in sectionList)
                        {
                            var coordinateSplitted = item.Split('$', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var latLong in coordinateSplitted)
                            {
                                var latLongSplitted = latLong.Split(' ');
                                coordinates.Add(new RouteCoordinates
                                {
                                    Longitude = latLongSplitted[0],
                                    Latitude = latLongSplitted[1]
                                });
                            }
                        }


                        var routeStep = _mapper.Map<RouteStep>(pathInfo);
                        routeStep.Coordinates = coordinates;
                        routeStep.StepIndex = (i + 1).ToString();

                        travelInformation.RouteSteps.Add(routeStep);
                    }

                    routeOption.TravelInformation = travelInformation;

                    routeOptions.Add(routeOption);
                }

                return Ok(routeOptions);
            }
            catch (FlurlHttpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}