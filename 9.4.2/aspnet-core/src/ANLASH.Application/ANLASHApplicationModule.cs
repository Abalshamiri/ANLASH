using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ANLASH.Authorization;

namespace ANLASH
{
    [DependsOn(
        typeof(ANLASHCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ANLASHApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ANLASHAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ANLASHApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
