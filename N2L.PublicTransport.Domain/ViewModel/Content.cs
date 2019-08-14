using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Domain.ViewModel
{
    public class Content
    {
        public string classe { get; set; }
        public List<NextBusHtml> div { get; set; }
    }

    public class ContentUnique
    {
        public string classe { get; set; }
        public NextBusHtml div { get; set; }
    }
}
