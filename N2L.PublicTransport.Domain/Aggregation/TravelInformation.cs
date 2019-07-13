using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Domain.Aggregation
{
    public class TravelInformation
    {
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string LeaveAt { get; set; }
        public string ArriveAt { get; set; }
        public string TransportTransfer { get; set; }
        public string FootPrintCarbon { get; set; }
        public string TimeDuration { get; set; }
        public string Price { get; set; }
        public string DistanceInMeters { get; set; }

        public List<RouteStep> RouteSteps { get; set; }

        public TravelInformation()
        {
            RouteSteps = new List<RouteStep>();
        }

    }
}
