namespace Identity.Business.Services.IdentityServers.Abstractions
{
    using System.Threading.Tasks;

    public interface IUserTypeProvider
    {
        Task<string> Create(string name, string displayName, string description);
        Task Delete(string userTypeProviderId);
        Task<bool> Update(string userTypeProviderId, string name, string displayName, string description);
    }
}