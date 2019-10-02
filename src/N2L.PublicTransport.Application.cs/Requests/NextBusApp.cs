using AutoMapper;
using N2L.PublicTransport.Application.Interfaces;
using N2L.PublicTransport.CrossCutting.Extensions;
using N2L.PublicTransport.Domain.Aggregation;
using N2L.PublicTransport.Domain.Entities;
using N2L.PublicTransport.Domain.ViewModel;
using N2L.PublicTransport.Infrastructure.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Application.Requests
{
    public class NextBusApp : INextBusApp
    {
        private readonly INextBusInfra _nextBusInfra;
        private readonly IMapper _mapper;

        public NextBusApp(INextBusInfra nextBusInfra, IMapper mapper)
        {
            _nextBusInfra = nextBusInfra;
            _mapper = mapper;
        }

        public async Task<List<Route>> GetRoutes(string startLatitude, string startLongitude, string endLatitude, string endLongitude, string data, string hora, bool isArrivalTime)
        {
            var responseMessage = await _nextBusInfra.GetRoutes(startLatitude, startLongitude, endLatitude, endLongitude, data, hora, isArrivalTime);

            List<Route> routeOptions = await ExtractContent(responseMessage);

            return routeOptions;

        }

        public async Task<List<TravelInformation>> GetTravelInformation(string startLatitude, string startLongitude, string[] destinations)
        {
            var responseList = _nextBusInfra.GetNextBus(startLatitude, startLongitude, destinations);
            var travelInfomrationList = new List<TravelInformation>();

            for (int i = 0; i < responseList.Count; i++)
            {
                var result = await ExtractContent(responseList[i], false);
                var travelInfo = result.FirstOrDefault().TravelInformation;
                travelInfo.ToLocation = destinations[i];
                travelInfo.FromLocation = $"{startLatitude},{startLongitude}";
                travelInfomrationList.Add(result.FirstOrDefault().TravelInformation);
            }
            return travelInfomrationList;
        }

        private async Task<List<Route>> ExtractContent(HttpResponseMessage response, bool coordinates = true)
        {
            var routes = JsonConvert.DeserializeObject<IEnumerable<RouteViewModel>>(await response.Content.ReadAsStringAsync());

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
                if (coordinates)
                    ExtractCoordinates(travelInformation, coordinatesSections, sectionInfo);

                routeOption.TravelInformation = travelInformation;
                routeOptions.Add(routeOption);
            }

            return routeOptions;
        }

        private void ExtractCoordinates(TravelInformation travelInformation, string[] coordinatesSections, string[] sectionInfo)
        {
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
        }

    }
}
