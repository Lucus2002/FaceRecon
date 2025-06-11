using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementApp.Models;

namespace SchoolManagementApp.Data
{
    public class SchoolManagementDbContext :IdentityDbContext<ApplicationUser>
    {
        public SchoolManagementDbContext(DbContextOptions<SchoolManagementDbContext> options)
           : base(options)
        {
        }

        public DbSet<Models.Person> Person { get; set; }
        public DbSet<PersonPhoto> PersonPhoto { get; set; }
    }
}
