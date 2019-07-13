using N2L.PublicTransport.Domain.Aggregation;
using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.Entities
{
    public class SearchStop
    {
        public IEnumerable<NextBus> NextBusList { get; set; }
        public IEnumerable<StopLocation> StopLocationList { get; set; }
    }

}
