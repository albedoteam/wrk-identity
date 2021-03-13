using System.Threading.Tasks;
using Identity.Business.Models;

namespace Identity.Business.Services.IdentityServers.Abstractions
{
    public interface IAuthServerProvider
    {
        Task<AuthServer> Create(string accountId, string accountName, string accountDisplayName);
        Task Activate(string authServerProviderId);
        Task Deactivate(string providerId);
        Task Delete(string providerId);
    }
}