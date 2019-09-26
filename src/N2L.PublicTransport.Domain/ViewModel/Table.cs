using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.ViewModel
{
    public class Table
    {
        public string _class { get; set; }
        public string border { get; set; }
        public string cellpadding { get; set; }
        public string cellspacing { get; set; }
        public object[] tr { get; set; }
    }
}
