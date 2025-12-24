using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;

namespace ANLASH.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly ANLASHDbContext _context;

        public DefaultSettingsCreator(ANLASHDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            // Set tenantId based on multi-tenancy configuration
            // If multi-tenancy is disabled, use default tenant; otherwise null for host settings
            int? tenantId = GetTenantIdForSeedData();

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer", tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);
        }

        /// <summary>
        /// Gets the appropriate tenant ID for seed data based on multi-tenancy configuration
        /// </summary>
        private int? GetTenantIdForSeedData()
        {
            // If multi-tenancy is disabled, seed data should belong to the default tenant
            // If enabled, seed data belongs to host (null tenant)
            if (!ANLASHConsts.MultiTenancyEnabled)
            {
                return MultiTenancyConsts.DefaultTenantId;
            }
            
            return null; // Host settings
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
