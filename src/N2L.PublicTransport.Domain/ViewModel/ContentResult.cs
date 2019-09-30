using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.ViewModel
{
    public class ContentResult
    {
        public string _class { get; set; }
        public string text { get; set; }
        public string label { get; set; }
        public string p { get; set; }
        public DivContent[] DivContents { get; set; }
        public Table table { get; set; }
        public A a { get; set; }
    }

}
