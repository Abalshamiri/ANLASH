using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ANLASH.Controllers
{
    public abstract class ANLASHControllerBase: AbpController
    {
        protected ANLASHControllerBase()
        {
            LocalizationSourceName = ANLASHConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
