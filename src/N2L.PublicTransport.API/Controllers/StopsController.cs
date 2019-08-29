using AutoMapper;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using N2L.PublicTransport.CrossCutting.Extensions;
using N2L.PublicTransport.Domain.Aggregation;
using N2L.PublicTransport.Domain.Entities;
using N2L.PublicTransport.Domain.ViewModel;
using System;
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
                if(splitResult[0] == "Erro")
                {
                    return StatusCode((int)System.Net.HttpStatusCode.Gone, new { data = splitResult[2] });
                }


                var htmlToJson = await htmlResult.ToJsonAutoMode();
                var responseHtmlParse = await htmlToJson.Content.ReadAsStringAsync();

                SearchStop searchStopList;
                try
                {
                    HtmlContent parseHtmlResult = Newtonsoft.Json.JsonConvert.DeserializeObject<HtmlContent>(responseHtmlParse.Replace("@", "").Replace("class", "classe").Replace("#text", "text"));
                    var stopList = parseHtmlResult.div.div;
                    var nextBus = _mapper.Map<IEnumerable<NextBus>>(stopList);
                    var stopLocation = _mapper.Map<IEnumerable<StopLocation>>(csvResult);

                     searchStopList = new SearchStop
                    {
                        StopLocationList = stopLocation,
                        NextBusList = nextBus
                    };
                }
                catch (Exception ex)
                {
                    HtmlContentUnique parseHtmlResult = Newtonsoft.Json.JsonConvert.DeserializeObject<HtmlContentUnique>(responseHtmlParse.Replace("@", "").Replace("class", "classe").Replace("#text", "text"));


                    var stopList = parseHtmlResult.div.div;
                    var nextBus = _mapper.Map<NextBus>(stopList);
                    var stopLocation = _mapper.Map<StopLocation>(csvResult[0]);

                     searchStopList = new SearchStop
                    {
                        StopLocationList = new List<StopLocation> { stopLocation },
                        NextBusList = new List<NextBus> { nextBus },
                    };

                }

                

               

                return Ok(searchStopList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}