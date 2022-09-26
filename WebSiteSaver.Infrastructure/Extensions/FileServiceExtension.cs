using System;
using System.Linq;

namespace WebSiteSaver.Infrastructure.Extensions
{
    internal static class FileServiceExtension
    {
        private const string SplashCharacter = "/";
        private const string IndexFileName = "index";
        private const string HtmlExtension = ".html";

        /// <summary>
        /// Get path for web page file including nested pages
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string GetFilePath(string outputPath, string fileName)
        {
            var parts = fileName.Split(SplashCharacter).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

            var htmlPageName = String.Empty;
            var subfolders = String.Empty;

            if (parts.Any())
            {
                htmlPageName = parts[^1];
                subfolders = string.Join(SplashCharacter, parts[..^1]);
            }
            else
            {
                htmlPageName = IndexFileName;
            }

            if (!htmlPageName.Contains(HtmlExtension))
            {
                htmlPageName += HtmlExtension;
            }

            return $"{outputPath}/{subfolders}/{htmlPageName}";
        }
    }
}
