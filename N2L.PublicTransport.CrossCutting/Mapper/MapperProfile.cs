using AutoMapper;
using N2L.PublicTransport.Domain.Aggregation;
using N2L.PublicTransport.Domain.Entities;
using N2L.PublicTransport.Domain.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace N2L.PublicTransport.CrossCutting.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<string, LineRoute>()

                .ForPath(dest => dest.StopCode, opt => opt.MapFrom(src => src.Split('|', StringSplitOptions.RemoveEmptyEntries)[0]))
                .ForPath(dest => dest.StopName, opt => opt.MapFrom(src => src.Split('|', StringSplitOptions.RemoveEmptyEntries)[1]))
                .ForPath(dest => dest.StopLatitude, opt => opt.MapFrom(src => src.Split('|', StringSplitOptions.RemoveEmptyEntries)[3]))
                .ForPath(dest => dest.StopLongitude, opt => opt.MapFrom(src => src.Split('|', StringSplitOptions.RemoveEmptyEntries)[2]))
                .ForPath(dest => dest.RoutGeolocationList, opt => opt.MapFrom(src => src.Split('|', StringSplitOptions.RemoveEmptyEntries)[4]
                .Split('$', StringSplitOptions.RemoveEmptyEntries)
                                            .Where(x => x.Contains("-9"))
                                            .Select(x => new RouteGeolocation
                                            {
                                                Latitude = x.Replace("#", "").Split(' ', StringSplitOptions.RemoveEmptyEntries)[0],
                                                Longitude = x.Replace("#", "").Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]
                                            })
                ));



            CreateMap<JToken, ValueInformation>()
                .ForPath(dest => dest.CarrierImage, opt => opt.MapFrom(src => src["CarrierImage"]))
                .ForPath(dest => dest.CarrierUrl, opt => opt.MapFrom(src => src["CarrierUrl"]))
                .ForPath(dest => dest.DetailLineUrl, opt => opt.MapFrom(src => src["DetailLineUrl"]))
                .ForPath(dest => dest.LineName, opt => opt.MapFrom(src => src["LineName"]));

            CreateMap<NextBusHtml, NextBus>()
                .ForPath(dest => dest.Image, opt => opt.MapFrom(src => src.div[0].img.src))
                .ForPath(dest => dest.Time, opt => opt
                             .MapFrom(src => $"{((Newtonsoft.Json.Linq.JContainer)src.div[1].label)[0].ToString()} {((Newtonsoft.Json.Linq.JContainer)src.div[1].text)[0].ToString().Replace("/", "")}"))
                .ForPath(dest => dest.Operator, opt => opt
                                                 .MapFrom(src => $"{((Newtonsoft.Json.Linq.JContainer)src.div[1].label)[1].ToString()} {((Newtonsoft.Json.Linq.JContainer)src.div[1].text)[1].ToString()}"))
                .ForPath(dest => dest.Code, opt => opt.MapFrom(src => $"{src.div[2].label} {src.div[2].text}"))
                .ForPath(dest => dest.Address, opt => opt.MapFrom(src => src.div[3].a.text))
                .ForPath(dest => dest.Line, opt => opt.MapFrom(src => src.div.Count > 4 ? src.div[4].a.text : String.Empty));


            CreateMap<string, StopLocation>()
                .ForPath(dest => dest.Latitude, opt => opt.MapFrom(src => GetValueByIndexPosition(src, 0)))
                .ForPath(dest => dest.Longitude, opt => opt.MapFrom(src => GetValueByIndexPosition(src, 1)))
                .ForPath(dest => dest.Address, opt => opt.MapFrom(src => GetValueByIndexPosition(src, 2)))
                .ForPath(dest => dest.StopCode, opt => opt.MapFrom(src => GetValueByIndexPosition(src, 3)));


            CreateMap<JObject, TravelInformation>()
                .ForPath(dest => dest.LeaveAt, opt => opt.MapFrom(src => src["div"][3]["div"][0]["p"].ToString()))
                .ForPath(dest => dest.ArriveAt, opt => opt.MapFrom(src => src["div"][3]["div"][1]["p"].ToString()))
                .ForPath(dest => dest.TimeDuration, opt => opt.MapFrom(src => src["div"][5]["div"][0]["p"].ToString()))
                .ForPath(dest => dest.Price, opt => opt.MapFrom(src => src["div"][5]["div"][1]["p"].ToString()))
                .ForPath(dest => dest.TransportTransfer, opt => opt.MapFrom(src => src["div"][4]["div"][0]["p"].ToString()))
                .ForPath(dest => dest.FootPrintCarbon, opt => opt.MapFrom(src => src["div"][4]["div"][1]["p"].ToString()))
                .ForPath(dest => dest.DistanceInMeters, opt => opt.MapFrom(src => src["div"][5]["div"][2]["p"].ToString()));


            CreateMap<string[], RouteStep>()
                .ForPath(dest => dest.LeaveTime, opt => opt.MapFrom(src => src[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]))
                .ForPath(dest => dest.ArriveTime, opt => opt.MapFrom(src => src[4].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]))
                .ForPath(dest => dest.Instruction, opt => opt.MapFrom(src => src[2]))
                .ForPath(dest => dest.TransportType, opt => opt.MapFrom(src => src[7]))
                .ForPath(dest => dest.TransportCarrier, opt => opt.MapFrom(src => src[5]));
        }




        private string GetValueByIndexPosition(string src, int index)
        {
            try
            {
                return src.Split('/')[index];
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
