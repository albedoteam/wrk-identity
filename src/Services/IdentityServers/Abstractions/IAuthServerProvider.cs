namespace Identity.Business.Services.IdentityServers.Abstractions
{
    using System.Threading.Tasks;
    using Models;

    public interface IAuthServerProvider
    {
        Task<AuthServer> Create(string accountId, string accountName, string accountDisplayName);
        Task Activate(string authServerProviderId);
        Task Deactivate(string providerId);
        Task Delete(string providerId);
    }
}