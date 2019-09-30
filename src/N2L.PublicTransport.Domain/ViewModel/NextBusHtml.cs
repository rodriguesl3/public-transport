using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Domain.ViewModel
{
    public class NextBusHtml
    {
        public string classe { get; set; }
        public List<NextBusDetails> div { get; set; }
    }

}
