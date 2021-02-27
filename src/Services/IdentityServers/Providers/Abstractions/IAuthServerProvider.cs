using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using Identity.Business.Models;

namespace Identity.Business.Services.IdentityServers.Providers.Abstractions
{
    public interface IAuthServerProvider
    {
        Task<AuthServer> Create(Provider provider, string accountId, string accountName, string accountDisplayName);
        Task<bool> Activate(string providerId);
        Task<bool> Deactivate(string providerId);
        Task<bool> Delete(string providerId);
    }
}