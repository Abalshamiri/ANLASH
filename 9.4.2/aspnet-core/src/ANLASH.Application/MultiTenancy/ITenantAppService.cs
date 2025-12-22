using Abp.Application.Services;
using ANLASH.MultiTenancy.Dto;

namespace ANLASH.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

