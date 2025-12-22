using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ANLASH.EntityFrameworkCore;
using ANLASH.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ANLASH.Web.Tests
{
    [DependsOn(
        typeof(ANLASHWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class ANLASHWebTestModule : AbpModule
    {
        public ANLASHWebTestModule(ANLASHEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ANLASHWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ANLASHWebMvcModule).Assembly);
        }
    }
}