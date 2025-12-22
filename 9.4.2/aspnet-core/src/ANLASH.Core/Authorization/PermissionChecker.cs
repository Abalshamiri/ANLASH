using Abp.Authorization;
using ANLASH.Authorization.Roles;
using ANLASH.Authorization.Users;

namespace ANLASH.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
