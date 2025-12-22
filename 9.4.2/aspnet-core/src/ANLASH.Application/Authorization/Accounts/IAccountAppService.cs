using System.Threading.Tasks;
using Abp.Application.Services;
using ANLASH.Authorization.Accounts.Dto;

namespace ANLASH.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
