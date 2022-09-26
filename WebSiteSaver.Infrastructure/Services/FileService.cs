using Microsoft.Extensions.Options;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebSiteSaver.Application.Interfaces;
using WebSiteSaver.Application.Models;
using WebSiteSaver.Infrastructure.Extensions;

namespace WebSiteSaver.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IOptions<SettingsModel> _settings;

        public FileService(IOptions<SettingsModel> settings)
        {
            _settings = settings;
        }

        public async Task CreateHtmlFile(string outputPath, PageModel pageFileModel)
        {
            var filePath = FileServiceExtension.GetFilePath(outputPath, pageFileModel.Name);
            
            CreateDirectory(Path.GetDirectoryName(filePath));

            await System.IO.File.WriteAllTextAsync(filePath, pageFileModel?.Body?.Text?.ToString());
        }

        public async Task DownloadPages(string outputPath,
            IEnumerable<PageModel> resources,
            ProgressBar progressBar,
            CancellationToken cancellationToken = default)
        {
            ParallelOptions parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Int32.Parse(_settings.Value.MaxDegreeOfParrallelism)
            };

            await Parallel.ForEachAsync(resources, cancellationToken, async (page, cancellationToken) =>
            {
                await CreateHtmlFile(outputPath, page);

                progressBar.Tick();
            });
        }

        private void CreateDirectory(string folderPath)
        {
            bool exists = System.IO.Directory.Exists(folderPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);
        }
    }
}
