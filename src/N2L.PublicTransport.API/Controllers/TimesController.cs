using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Flurl.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using N2L.PublicTransport.Domain.Entities;

namespace N2L.PublicTransport.API.Controllers
{



    public static class FileReader
    {
        public static object ReadJsonFile(string filePath)
        {
            var file = String.Join("", File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("iso-8859-1")));
            var linesAavailables = Newtonsoft.Json.JsonConvert.DeserializeObject(file);
            var response = ((Newtonsoft.Json.Linq.JToken)linesAavailables)["data"];

            return response;
        }
    }

    [Route("api/time")]
    [ApiController]
    public class TimesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly string pathToFile;
        public TimesController(IMapper mapper, IHostingEnvironment env)
        {
            _mapper = mapper;
            pathToFile = $"{env.ContentRootPath}{Path.DirectorySeparatorChar.ToString()}{"linesAvailable.json"}";
        }

        [HttpGet]
        [Route("carriers")]
        public async Task<IActionResult> GetLines(string searchQuery = null)
        {
            Func<ValueInformation, bool> expression;
            if (string.IsNullOrEmpty(searchQuery))
                expression = x => x.CarrierUrl.Contains("Carris") || x.CarrierUrl.Contains("CP") || x.CarrierUrl.Contains("METRO");
            else
                expression = x => x.CarrierUrl.Contains(searchQuery) || x.LineName.Contains(searchQuery);


            var lines = FileReader.ReadJsonFile(pathToFile);
            var values = _mapper.Map<IEnumerable<ValueInformation>>(lines).Where(expression);
                
            return Ok(new { data = values });
        }

        [HttpGet]
        [Route("update")]
        public async Task<IActionResult> UpdateLines()
        {
            var baseUrl = Environment.GetEnvironmentVariable("LISBON_DOMAIN");
            var informationList = new List<ValueInformation>();
            var keyValueInfo = new Dictionary<string, string>();
            var keyValueDetailInfo = new Dictionary<string, object>();

            var response = await $"{baseUrl}Default.aspx?tabid=191&language=pt-PT".GetAsync();

            (await response.Content.ReadAsStringAsync())
                .Split(new[] { "meios_transp_options" }, StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(new[] { "/right" }, StringSplitOptions.RemoveEmptyEntries)[0]
                .Split(new[] { "a href=" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains("><img src="))
                .ToList()
                .ForEach((x) =>
                {
                    var url = x.Split(new[] { "><img src=\"" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("\"", "");

                    var detailResponse = $"{baseUrl}{url}".GetAsync().GetAwaiter().GetResult();
                    var responseParsed = detailResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    responseParsed.Split(new[] { "<TABLE class=\"timetable\" " }, StringSplitOptions.RemoveEmptyEntries)[1]
                                                 .Split(new[] { "</TABLE>" }, StringSplitOptions.RemoveEmptyEntries)[0]
                                                 .Split(new[] { "<TD><a href=" }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Where(p => p.StartsWith("\""))
                                                 .Select(p => p.Split(new[] { "</a>" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("\"", "")
                                                               .Split(new[] { ">" }, StringSplitOptions.RemoveEmptyEntries)).ToList()
                                                 .ForEach(k =>
                                                 {
                                                     informationList.Add(new ValueInformation
                                                     {
                                                         CarrierUrl = url,
                                                         CarrierImage = x.Split(new[] { "><img src=\"" }, StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, x.Split(new[] { "><img src=\"" }, StringSplitOptions.RemoveEmptyEntries)[1].IndexOf('"')),
                                                         DetailLineUrl = k[0],
                                                         LineName = k[1]
                                                     });

                                                 });
                }
                );


            return Ok(informationList);
        }

        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> GetLineDetail(string codOp, string lineId, string hour, string minutes, string date)
        {
            var response = await $"{Environment.GetEnvironmentVariable("LISBON_DOMAIN")}/DesktopModules/trp_horarios/Ajax/trp_horario_2.ashx?cmd=getLine&codOp={codOp}&lineId={lineId}&hora={hour}&min={minutes}&date={date}".GetAsync();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var splitResponse = stringResponse.Split(new[] { "#--#" }, StringSplitOptions.RemoveEmptyEntries);

            var coordinates = splitResponse[2].Split(new[] { "#*##-#" }, StringSplitOptions.RemoveEmptyEntries);

            var resultMap = _mapper.Map<List<LineRoute>>(coordinates);

            resultMap.Add(new LineRoute
            {
                StopCode = coordinates[4].Split('|')[0],
                StopName = coordinates[4].Split('|')[1],
                StopLatitude = coordinates[4].Split('|')[2],
                StopLongitude = coordinates[4].Split('|')[3]
            });

            return Ok(resultMap);
        }
    }
}