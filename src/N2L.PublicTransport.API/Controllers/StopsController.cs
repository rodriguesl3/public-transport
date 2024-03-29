﻿using AutoMapper;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using N2L.PublicTransport.CrossCutting.Extensions;
using N2L.PublicTransport.Domain.Aggregation;
using N2L.PublicTransport.Domain.Entities;
using N2L.PublicTransport.Domain.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N2L.PublicTransport.API.Controllers
{
    [Route("api/stops")]
    [ApiController]
    public class StopsController : Controller
    {

        private readonly IMapper _mapper;

        public StopsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        private double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calaculate distance in metres.
            distance = radius * c;
            return distance;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string latitude, string longitude, string data, string hora)
        {
            try
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

                var result = await response.Content.ReadAsStringAsync();
                var splitResult = result.Split(new[] { "#___#" }, StringSplitOptions.RemoveEmptyEntries);
                var csvResult = splitResult[1].Split('*');
                var htmlResult = splitResult[0];
                if (splitResult[0] == "Erro")
                {
                    return StatusCode((int)System.Net.HttpStatusCode.Gone, new { data = splitResult[2] });
                }


                var htmlToJson = await htmlResult.ToJsonAutoMode();
                var responseHtmlParse = await htmlToJson.Content.ReadAsStringAsync();

                SearchStop searchStopList = new SearchStop();
                IEnumerable<StopLocation> stopLocationList;
                IEnumerable<NextBus> nextBusList;

                try
                {
                    HtmlContent parseHtmlResult = Newtonsoft.Json.JsonConvert.DeserializeObject<HtmlContent>(responseHtmlParse.Replace("@", "").Replace("class", "classe").Replace("#text", "text"));
                    var stopList = parseHtmlResult.div.div;
                    nextBusList = _mapper.Map<IEnumerable<NextBus>>(stopList);
                    stopLocationList = _mapper.Map<IEnumerable<StopLocation>>(csvResult);
                }
                catch (Exception ex)
                {
                    HtmlContentUnique parseHtmlResult = Newtonsoft.Json.JsonConvert.DeserializeObject<HtmlContentUnique>(responseHtmlParse.Replace("@", "").Replace("class", "classe").Replace("#text", "text"));


                    var stopList = parseHtmlResult.div.div;
                    var nextBus = _mapper.Map<NextBus>(stopList);
                    var stopLocation = _mapper.Map<StopLocation>(csvResult[0]);
                    stopLocationList = new List<StopLocation> { stopLocation };
                    nextBusList = new List<NextBus> { nextBus };
                }


                List<StopLocation> resultList = new List<StopLocation>();

                foreach (var stop in stopLocationList)
                {
                    stop.NextBuses = nextBusList.Where(x => x.Code.Contains(stop.StopCode));

                    resultList.Add(stop);
                }

                resultList = resultList.OrderBy(x => GetDistanceBetweenPoints(Convert.ToDouble(x.Latitude), Convert.ToDouble(x.Longitude), Convert.ToDouble(latitude), Convert.ToDouble(longitude))).ToList();

                return Ok(resultList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}