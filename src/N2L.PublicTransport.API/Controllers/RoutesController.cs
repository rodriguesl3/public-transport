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
using System.Net.Http;
using N2L.PublicTransport.Application.Interfaces;

namespace N2L.PublicTransport.API.Controllers
{
    [Route("api/routes")]
    public class RoutesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INextBusApp _nextBusApp;
        public RoutesController(IMapper mapper, INextBusApp nextBusApp)
        {
            _mapper = mapper;
            _nextBusApp = nextBusApp;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string startLatitude, string startLongitude,
            string endLatitude, string endLongitude,
            string data, string hora, bool isArrivalTime)
        {
            try
            {
                var routeOptions = await _nextBusApp.GetRoutes(startLatitude, startLongitude, endLatitude, endLongitude, data, hora, isArrivalTime);
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


        [HttpGet]
        [Route("next")]
        public async Task<IActionResult> GetNextBus(string startLatitude, string startLongitude, string[] destinations)
        {
            var travelInformationList = await _nextBusApp.GetTravelInformation(startLatitude, startLongitude, destinations);
            return Ok(new { data = travelInformationList });
        }
    }
}