using HtmlAgilityPack;
using ShellProgressBar;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebSiteSaver.Application.Models;

namespace WebSiteSaver.Application.Interfaces
{
    public interface IHtmlService
    {
        Task<HtmlDocument> GetHtmlBody(string url, 
            CancellationToken cancellationToken = default);

        Task<IEnumerable<PageModel>> GetPages(string basicUrl,
            IEnumerable<string> links,
            ProgressBar progressBar,
            CancellationToken cancellationToken = default);
    }
}
