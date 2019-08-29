using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.Entities
{
    public class LineRoute
    {
        public string StopCode { get; set; }
        public string StopName { get; set; }
        public string StopLatitude { get; set; }
        public string StopLongitude { get; set; }
        public IEnumerable<RouteGeolocation> RoutGeolocationList { get; set; }
    }

    public class RouteGeolocation
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

}
