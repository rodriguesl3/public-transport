using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.Aggregation
{
    public class NextBus
    {
        public string Image { get; set; }
        public string Time { get; set; }
        public string Operator { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Line { get; set; }
    }
}
