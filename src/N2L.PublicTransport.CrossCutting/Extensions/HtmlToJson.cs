using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace N2L.PublicTransport.CrossCutting.Extensions
{
    public static class HtmlToJson
    {
        public static async Task<HttpResponseMessage> ToJsonAutoMode(this string html)
        {
            return await Environment.GetEnvironmentVariable("PARSE_HTML").PostMultipartAsync(mp =>
                    mp
                    .AddString("tool", "data-html-to-json-converter")
                    .AddJson("parameters", new
                    {
                        indent = true,
                        unescapeJson = true,
                        mode = "Auto",
                        attributePrefix = "@",
                        textPropertyName = "#text",
                        input = html
                    }));

        }


        public static async Task<HttpResponseMessage> ToJsonGenericMode(this string html)
        {
            return await Environment.GetEnvironmentVariable("PARSE_HTML").PostMultipartAsync(mp =>
                    mp
                    .AddString("tool", "data-html-to-json-converter")
                    .AddJson("parameters", new
                    {
                        indent = true,
                        unescapeJson = true,
                        mode = "Generic",
                        attributePrefix = "@",
                        textPropertyName = "#text",
                        input = html
                    }));

        }
    }
}
