namespace ANLASH.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly ANLASHDbContext _context;

        public InitialHostDbBuilder(ANLASHDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            
            // Seed Universities ✨
            new InitialUniversitiesCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
