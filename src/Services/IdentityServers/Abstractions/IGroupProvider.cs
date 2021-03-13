using System.Threading.Tasks;

namespace Identity.Business.Services.IdentityServers.Abstractions
{
    public interface IGroupProvider
    {
        Task<string> Create(string name, string description);
        Task Delete(string groupProviderId);
        Task<bool> Update(string groupProviderId, string name, string description);
    }
}