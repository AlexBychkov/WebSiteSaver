namespace WebSiteSaver.Application.Models
{
    public class SettingsModel
    {
        public const string SectionName = "Main";
        
        /// <summary>
        /// Initial web site Url to start traversing
        /// </summary>
        public string BaseWebSiteUrl { get; set; }

        /// <summary>
        /// How many streams system will use to parallel actions
        /// </summary>
        public string MaxDegreeOfParrallelism  { get; set; }

        /// <summary>
        /// Folder where to put files
        /// </summary>
        public string OutputFolderName { get; set; }
        
    }
}
