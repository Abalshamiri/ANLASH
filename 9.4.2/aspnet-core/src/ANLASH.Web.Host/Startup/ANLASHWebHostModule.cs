using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ANLASH.Configuration;

namespace ANLASH.Web.Host.Startup
{
    [DependsOn(
       typeof(ANLASHWebCoreModule))]
    public class ANLASHWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ANLASHWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ANLASHWebHostModule).GetAssembly());
        }
    }
}
