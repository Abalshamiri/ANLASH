using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ANLASH.Configuration.Dto;

namespace ANLASH.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ANLASHAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
