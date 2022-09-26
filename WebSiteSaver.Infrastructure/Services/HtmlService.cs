using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using ShellProgressBar;
using WebSiteSaver.Application.Interfaces;
using WebSiteSaver.Application.Models;

namespace WebSiteSaver.Infrastructure.Services
{
    public class HtmlService : IHtmlService
    {
        private readonly IOptions<SettingsModel> _settings;

        public HtmlService(IOptions<SettingsModel> settings)
        {
            _settings = settings;
        }

        async public Task<HtmlDocument> GetHtmlBody(string url, 
            CancellationToken cancellationToken = default)
        {
            HtmlWeb web = new HtmlWeb();

            return await web.LoadFromWebAsync(url);
        }

        async public Task<IEnumerable<PageModel>> GetPages(string basicUrl,
                IEnumerable<string> links,
                ProgressBar progressBar,
                CancellationToken cancellationToken = default)
        {
            var pages = new List<PageModel>();

            ParallelOptions parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Int32.Parse(_settings.Value.MaxDegreeOfParrallelism)
            };

            await Parallel.ForEachAsync(links, parallelOptions, async (url, token) =>
            {
                var htmlDoc = await GetHtmlBody(basicUrl + url);

                pages.Add(new PageModel() { Name = url, Body = htmlDoc });

                progressBar.Tick();

            });

            return pages;
        }
    }
}
