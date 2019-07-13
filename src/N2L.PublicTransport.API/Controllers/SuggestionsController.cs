using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using N2L.PublicTransport.Domain.Entities;

namespace N2L.PublicTransport.API.Controllers
{
    [Route("api/suggestions")]
    [ApiController]
    public class SuggestionsController : Controller
    {
        public IActionResult Index()
        {
            var suggestionsList = new List<Suggestions>
            {
                new Suggestions("https://www.aworldtotravel.com/wp-content/uploads/2017/07/baixa-and-castle-things-lisbon-is-famous-for-a-world-to-travel.jpg",
                "Castelo de São Jorge",
                "A journey here provides you an experience like no other. Be amazed at the beauty of the medieval architecture and view you get from the castle, which sits prettily on top of Alfama’s hill."
                ),
                new Suggestions("https://www.aworldtotravel.com/wp-content/uploads/2017/07/baixa-and-castle-things-lisbon-is-famous-for-a-world-to-travel.jpg",
                "Castelo de São Jorge",
                "A journey here provides you an experience like no other. Be amazed at the beauty of the medieval architecture and view you get from the castle, which sits prettily on top of Alfama’s hill."
                ),
                new Suggestions("https://i1.wp.com/www.aworldtotravel.com/wp-content/uploads/2017/07/portuguese-custard-tart-things-lisbon-is-famous-for-a-world-to-travel.jpg?zoom=1.25&w=515&h=281&ssl=1",
                "PASTEIS DE BELEM – LISBON SWEETS OF CHOICE",
                "Food is huge here in Lisbon, and it doesn’t get any better than the famed Portuguese egg tarts, specifically those served here in Pasteis de Belem."
                ),
            };

            return Ok(suggestionsList);
        }
    }

}