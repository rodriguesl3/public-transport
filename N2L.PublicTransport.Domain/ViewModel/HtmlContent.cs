using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2L.PublicTransport.Domain.ViewModel
{

    public interface IHtmlParse
    {

    }

    public class HtmlContent:IHtmlParse
    {
        public Content div { get; set; }
    }

    public class HtmlContentUnique:IHtmlParse
    {
        public ContentUnique div { get; set; }
    }
}
