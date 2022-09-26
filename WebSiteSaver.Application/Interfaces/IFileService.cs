using ShellProgressBar;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebSiteSaver.Application.Models;

namespace WebSiteSaver.Application.Interfaces
{
    public interface IFileService
    {
        Task CreateHtmlFile(string outputPath, PageModel pageFileModel);

        Task DownloadPages(string outputPath,
            IEnumerable<PageModel> resources,
            ProgressBar progressBar,
            CancellationToken cancellationToken = default);
    }
}
