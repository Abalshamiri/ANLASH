using System.Threading.Tasks;
using Abp.Application.Services;
using ANLASH.Sessions.Dto;

namespace ANLASH.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
