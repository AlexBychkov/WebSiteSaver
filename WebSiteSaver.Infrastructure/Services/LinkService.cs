using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSiteSaver.Application.Interfaces;
using WebSiteSaver.Application.Models;

namespace WebSiteSaver.Infrastructure.Services
{
    public class LinkService : ILinkService
    {
        private const string LinkTag = "a";
        private const string LinkAttr = "href";
        private const string SplashCharacter = "/";
        private const string HashCharacter = "#";

        private readonly List<string> _links = new List<string>();
        private readonly IOptions<SettingsModel> _settings;


        IHtmlService _htmlService;

        public LinkService(IHtmlService htmlService, IOptions<SettingsModel> settings)
        { 
            _htmlService = htmlService;
            _settings = settings;
        }

        public async Task<IEnumerable<string>> GetLinksOnPageAsync(string websiteAddress, CancellationToken cancellationToken = default)
        {
            await GetLinks(websiteAddress, SplashCharacter, cancellationToken);

            return _links;
        }

        // The recursion is used to get nested links inside of the pages if there are any
        private async Task GetLinks(string websiteAddress, string pageUrl, CancellationToken cancellationToken = default)
        {
            ParallelOptions parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Int32.Parse(_settings.Value.MaxDegreeOfParrallelism)
            };

            var htmlDoc = await _htmlService.GetHtmlBody(websiteAddress + pageUrl, cancellationToken);

            var links = htmlDoc.DocumentNode.Descendants(LinkTag)
                                             .Select(a => a.GetAttributeValue(LinkAttr, null))
                                             .Where(u => !String.IsNullOrEmpty(u) && u.StartsWith(SplashCharacter) && !u.Contains(HashCharacter))
                                             .Select(a => a)
                                             .Distinct();

            await Parallel.ForEachAsync(links, parallelOptions, async (pageUrl, cancellationToken) =>
            {
                if (!_links.Contains(pageUrl))
                {
                    _links.Add(pageUrl);
                    await GetLinks(websiteAddress, pageUrl, cancellationToken);
                }
            });
        }

    }
}
