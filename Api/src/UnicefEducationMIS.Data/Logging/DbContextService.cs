using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnicefEducationMIS.Core.ApplicationServices;

namespace UnicefEducationMIS.Data.Logging
{
    public class DbContextService : IDbContextService
    {
        private UnicefEduDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public DbContextService(IConfiguration configuration,IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }
        public UnicefEduDbContext GetContext()
        {
            if (_context == null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentLoginUserService>();

                    var optionsBuilder = new DbContextOptionsBuilder<UnicefEduDbContext>();
                    optionsBuilder.UseMySQL(_configuration.GetConnectionString("unicefedudb_connection"));
                    _context = new UnicefEduDbContext(optionsBuilder.Options, currentUser);
                }
            }

            return _context;
        }
    }
}
