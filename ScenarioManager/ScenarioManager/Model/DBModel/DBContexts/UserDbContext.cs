using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel.DBContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserLoginInfo> Users { get; set; }
        public DbSet<TokenGuid> TokenGuids { get; set; }
    }
}
