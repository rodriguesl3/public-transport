using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flurl;
using Flurl.Http;

namespace N2L.PublicTransport.API.Controllers
{
    [Route("api/stops")]
    [ApiController]
    public class StopsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string latitude, string longitude, string data, string hora)
        {
            try
            {
                var response = await Environment.GetEnvironmentVariable("LISBON_API")
                .PostMultipartAsync(mp =>
                mp.AddString("cmd", "pesquisarParagem")
                  .AddString("coordenadas", $"{latitude}/{longitude}")
                  .AddString("hora", $"{hora}")
                  .AddString("data", $"{data}")
                  .AddString("UrlBase", Environment.GetEnvironmentVariable("LISBON_DOMAIN"))
                  .AddString("areaInfluencia", "100")
                  .AddString("intervalo", "1800")
                  .AddString("textLocal", "Definido no mapa")
                  .AddString("codOperador", "")
                );

                var result = await response.Content.ReadAsStringAsync();
                var splitResult = result.Split(new[] { "#___#" }, StringSplitOptions.RemoveEmptyEntries);
                var csvResult = splitResult[1];
                var htmlResult = splitResult[0];

                var htmlToJson = await Environment.GetEnvironmentVariable("PARSE_HTML").PostMultipartAsync(mp =>
                    mp
                    .AddString("tool", "data-html-to-json-converter")
                    .AddJson("parameters", new { indent = true, unescapeJson = true, mode = "Auto", attributePrefix = "@", textPropertyName = "#text", input = htmlResult }));

                var parseHtmlResult = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>((await htmlToJson.Content.ReadAsStringAsync()).Replace("@", "").Replace("class", "classe").Replace("#text", "text"));

                var stopList = parseHtmlResult.div.div;



                return Ok(stopList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public class Img
    {
        public string alt { get; set; }
        public string src { get; set; }
    }

    public class A
    {
        public string classe { get; set; }
        public string href { get; set; }
        public string text { get; set; }
    }

    public class Div3
    {
        public Img img { get; set; }
        public object label { get; set; }
        public object text { get; set; }
        public A a { get; set; }
    }

    public class Div2
    {
        public string classe { get; set; }
        public List<Div3> div { get; set; }
    }

    public class Div
    {
        public string classe { get; set; }
        public List<Div2> div { get; set; }
    }

    public class RootObject
    {
        public Div div { get; set; }
    }
}