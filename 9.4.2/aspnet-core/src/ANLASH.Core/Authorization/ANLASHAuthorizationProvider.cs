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

            // Language Centers ✨ NEW!
            var languageCenters = context.CreatePermission(PermissionNames.Pages_LanguageCenters, L("LanguageCenters"));
            languageCenters.CreateChildPermission(PermissionNames.Pages_LanguageCenters_Create, L("CreateLanguageCenter"));
            languageCenters.CreateChildPermission(PermissionNames.Pages_LanguageCenters_Edit, L("EditLanguageCenter"));
            languageCenters.CreateChildPermission(PermissionNames.Pages_LanguageCenters_Delete, L("DeleteLanguageCenter"));

            var languageCourses = context.CreatePermission(PermissionNames.Pages_LanguageCourses, L("LanguageCourses"));
            languageCourses.CreateChildPermission(PermissionNames.Pages_LanguageCourses_Create, L("CreateLanguageCourse"));
            languageCourses.CreateChildPermission(PermissionNames.Pages_LanguageCourses_Edit, L("EditLanguageCourse"));
            languageCourses.CreateChildPermission(PermissionNames.Pages_LanguageCourses_Delete, L("DeleteLanguageCourse"));

            var coursePricing = context.CreatePermission(PermissionNames.Pages_CoursePricing, L("CoursePricing"));
            coursePricing.CreateChildPermission(PermissionNames.Pages_CoursePricing_Create, L("CreateCoursePricing"));
            coursePricing.CreateChildPermission(PermissionNames.Pages_CoursePricing_Edit, L("EditCoursePricing"));
            coursePricing.CreateChildPermission(PermissionNames.Pages_CoursePricing_Delete, L("DeleteCoursePricing"));

            var languageCenterFAQs = context.CreatePermission(PermissionNames.Pages_LanguageCenterFAQs, L("LanguageCenterFAQs"));
            languageCenterFAQs.CreateChildPermission(PermissionNames.Pages_LanguageCenterFAQs_Create, L("CreateLanguageCenterFAQ"));
            languageCenterFAQs.CreateChildPermission(PermissionNames.Pages_LanguageCenterFAQs_Edit, L("EditLanguageCenterFAQ"));
            languageCenterFAQs.CreateChildPermission(PermissionNames.Pages_LanguageCenterFAQs_Delete, L("DeleteLanguageCenterFAQ"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ANLASHConsts.LocalizationSourceName);
        }
    }
}
