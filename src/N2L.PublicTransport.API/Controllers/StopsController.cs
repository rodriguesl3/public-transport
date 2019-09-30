using AutoMapper;
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
using N2L.PublicTransport.Application.Interfaces;

namespace N2L.PublicTransport.API.Controllers
{
    [Route("api/stops")]
    [ApiController]
    public class StopsController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IStopsApp _stopsApp;

        public StopsController(IStopsApp stopsApp)
        {
            _stopsApp = stopsApp;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string latitude, string longitude, string data, string hora)
        {
            try
            {
                var result = await _stopsApp.GetStopsLocations(latitude, longitude, data, hora);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}