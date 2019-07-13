using N2L.PublicTransport.Domain.Aggregation;
using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.Entities
{
    public class Route
    {
        public IEnumerable<TravelType> TravelTypes { get; set; }
        public string TravelTime { get; set; }
        public TravelInformation TravelInformation { get; set; }
    }
}
