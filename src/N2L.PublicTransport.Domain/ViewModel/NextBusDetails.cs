using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Domain.ViewModel
{
    public class NextBusDetails
    {
        public CarrierImage img { get; set; }
        public object label { get; set; }
        public object text { get; set; }
        public Anchor a { get; set; }
    }
}
