using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.ViewModel
{
    public class HTMLResult
    {
        public IEnumerable<ContentResult> Content { get; set; }
        public Input Input { get; set; }
    }
}
