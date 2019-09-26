using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.Aggregation
{
    public class RouteStep
    {
        public string StepIndex { get; set; }
        public string LeaveTime { get; set; }
        public string ArriveTime { get; set; }
        public string WaitTime { get; set; }
        public string Instruction { get; set; }
        public string TransportType { get; set; }
        public string TransportCarrier { get; set; }

        public IEnumerable<RouteCoordinates> Coordinates { get; set; }

    }
}
