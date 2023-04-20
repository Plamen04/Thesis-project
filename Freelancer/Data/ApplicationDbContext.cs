using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Freelancer.Models;
using Microsoft.AspNetCore.Identity;

namespace Freelancer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Job>? Job { get; set; }
        public DbSet<JobType> jobTypes { get; set; }
        public DbSet<Order> orders { get; set; }
    }
}