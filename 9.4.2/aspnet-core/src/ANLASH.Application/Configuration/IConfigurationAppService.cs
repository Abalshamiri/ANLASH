using System.Threading.Tasks;
using ANLASH.Configuration.Dto;

namespace ANLASH.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
