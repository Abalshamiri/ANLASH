using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ANLASH.Authorization
{
    public class ANLASHAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            // Universities ✨
            var universities = context.CreatePermission(PermissionNames.Pages_Universities, L("Universities"));
            universities.CreateChildPermission(PermissionNames.Pages_Universities_Create, L("CreateUniversity"));
            universities.CreateChildPermission(PermissionNames.Pages_Universities_Edit, L("EditUniversity"));
            universities.CreateChildPermission(PermissionNames.Pages_Universities_Delete, L("DeleteUniversity"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ANLASHConsts.LocalizationSourceName);
        }
    }
}
