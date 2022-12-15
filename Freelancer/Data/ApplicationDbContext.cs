using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Freelancer.Models;

namespace Freelancer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Freelancer.Models.User>? User { get; set; }
        public DbSet<Freelancer.Models.Jobs>? Job { get; set; }
    }
}