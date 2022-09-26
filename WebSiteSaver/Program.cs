using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebSiteSaver.Application.Interfaces;
using WebSiteSaver.Application.Models;
using WebSiteSaver.Application.Services;
using WebSiteSaver.Infrastructure.Services;

public static class Program
{
    private const string AppSettingsFileName = "appsettings.json";

    public static async Task Main(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();

        IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile(AppSettingsFileName, false)
                .Build();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<IHtmlService, HtmlService>();
                services.AddTransient<IFileService, FileService>();
                services.AddTransient<ILinkService, LinkService>();

                services.AddTransient<WebParserService>();

                services.Configure<SettingsModel>(config.GetSection(SettingsModel.SectionName));
            })
            .Build();

        await RunAppAsync(host.Services, cancellationTokenSource.Token);
        await host.RunAsync(cancellationTokenSource.Token);
    }

    private static async Task RunAppAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var serviceScope = services.CreateAsyncScope();
        var provider = serviceScope.ServiceProvider;
        var appService = provider.GetRequiredService<WebParserService>();

        await appService.TraverseUrlAsync(cancellationToken);
    }
}
