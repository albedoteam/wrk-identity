namespace Identity.Business.Services.IdentityServers.Abstractions
{
    using System.Threading.Tasks;

    public interface IGroupProvider
    {
        Task<string> Create(string name, string description);
        Task Delete(string groupProviderId);
        Task<bool> Update(string groupProviderId, string name, string description);
    }
}