using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.Aggregation
{
    public class StopLocation
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string StopCode { get; set; }
        public IEnumerable<NextBus> NextBuses { get; set; }
    }
}
