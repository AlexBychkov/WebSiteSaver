using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ShellProgressBar;
using WebSiteSaver.Application.Interfaces;
using WebSiteSaver.Application.Models;
using WebSiteSaver.Application.Services;

namespace WebSiteSaver.Application.Tests
{
    [TestFixture]
    public class WebSiteSaverApplicationUnitTests
    {
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IHtmlService> _htmlServiceMock;
        private readonly Mock<ILinkService> _linkServiceMock;
        private readonly Mock<IOptions<SettingsModel>> _settingsMock;

        private readonly WebParserService _webParserService;

        public WebSiteSaverApplicationUnitTests()
        {
            _fileServiceMock = new Mock<IFileService>();
            _htmlServiceMock = new Mock<IHtmlService>();
            _linkServiceMock = new Mock<ILinkService>();
            _settingsMock = new Mock<IOptions<SettingsModel>>();

            _webParserService = new WebParserService(_fileServiceMock.Object,
                _htmlServiceMock.Object,
                _linkServiceMock.Object,
                _settingsMock.Object);
        }

        [Test]
        public async Task RunApp_ShouldReturnSuccessfullMessage_IfLinksAndPagesAreProvided()
        {
            var links = new List<string>
            {
                "google.com",
                "yahoo.com",
                "bing.com"
            };

            _settingsMock.Setup(ap => ap.Value).Returns(new SettingsModel
            {
                BaseWebSiteUrl = "baseWebSiteUrl",
                OutputFolderName = "outputFolderName"
            });

            _linkServiceMock
                .Setup(x => x.GetLinksOnPageAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(links);

            var pages = new List<PageModel>() {
                new PageModel() { Name="Google", Body=new HtmlAgilityPack.HtmlDocument()}
             };

            _htmlServiceMock
                .Setup(x => x.GetPages(It.IsAny<string>(), links, It.IsAny<ProgressBar>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pages);

            Func<Task> func = async () => await _webParserService.TraverseUrlAsync();

            await func.Should().NotThrowAsync();
        }


        // It's hard to test failures, cause the app is not sending exceptions.
        // It's writting message to console
        [Test]
        public async Task RunApp_ShouldReturnLinksNotFound_IfNoLinksProvided()
        {
            var links = new List<string>();

            _settingsMock.Setup(ap => ap.Value).Returns(new SettingsModel
            {
                BaseWebSiteUrl = "baseWebSiteUrl",
                OutputFolderName = "outputFolderName"
            });

            _linkServiceMock
                .Setup(x => x.GetLinksOnPageAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(links);

            _htmlServiceMock
                .Setup(x => x.GetPages(It.IsAny<string>(), links, It.IsAny<ProgressBar>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PageModel>());

            var func = async () => await _webParserService.TraverseUrlAsync();

            await func.Should().NotThrowAsync();
        }
    }
}