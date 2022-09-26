using HtmlAgilityPack;

namespace WebSiteSaver.Application.Models
{
    public class PageModel
    {
        public string Name { get; set; }

        public HtmlDocument Body { get; set; }
    }
}
