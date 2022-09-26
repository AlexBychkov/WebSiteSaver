using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebSiteSaver.Application.Interfaces
{
    public interface ILinkService
    {
        Task<IEnumerable<string>> GetLinksOnPageAsync(string websiteAddress, CancellationToken cancellationToken = default);
    }
}
