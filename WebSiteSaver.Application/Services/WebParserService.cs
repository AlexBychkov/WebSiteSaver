using Microsoft.Extensions.Options;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSiteSaver.Application.Interfaces;
using WebSiteSaver.Application.Models;

namespace WebSiteSaver.Application.Services
{
    public class WebParserService
    {
        private readonly IFileService _fileService;
        private readonly IHtmlService _htmlService;
        private readonly ILinkService _linkService;
        private readonly IOptions<SettingsModel> _settings;

        public WebParserService(IFileService fileService, 
            IHtmlService htmlService,
            ILinkService linkService,
            IOptions<SettingsModel> settings)
        {
            _fileService = fileService;
            _htmlService = htmlService;
            _linkService = linkService;
            _settings = settings;
        }

        async public Task TraverseUrlAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var basicUrl = _settings.Value.BaseWebSiteUrl;
                var outputPath = _settings.Value.OutputFolderName;

                if (string.IsNullOrWhiteSpace(basicUrl))
                {
                    throw new Exception("Please, provide a valid basic url in appsettings.json");
                }

                var links = await _linkService.GetLinksOnPageAsync(basicUrl, cancellationToken);
                var linksCount = links.Count();

                if (linksCount == 0)
                {
                    throw new Exception("No links have been found on provided webpage");
                }

                IEnumerable<PageModel> pages;

                using (var progressBar = new ProgressBar(links.Count(), "Collecting all the pages..."))
                {
                    pages = await _htmlService.GetPages(basicUrl, links, progressBar, cancellationToken);
                }

                if (!pages.Any())
                {
                    throw new Exception("No pages have been found");
                }

                using (var progressBar = new ProgressBar(links.Count(), "Saving all the pages..."))
                {
                    await _fileService.DownloadPages(outputPath, pages, progressBar, cancellationToken);
                }

                Console.WriteLine($"Execution finished successfully. All the files could be found here: {Environment.CurrentDirectory}\\{outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Execution failed. Error : {ex.Message}");
            }
        }
    }
}
