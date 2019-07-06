using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace N2L.PublicTransport.API.Controllers
{
    public class RoutesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}