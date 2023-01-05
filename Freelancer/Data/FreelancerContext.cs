using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Freelancer.Models;

namespace Freelancer.Data
{
    public class FreelancerContext : DbContext
    {
        public FreelancerContext (DbContextOptions<FreelancerContext> options)
            : base(options)
        {
        }

        public DbSet<Freelancer.Models.Job> Job { get; set; } = default!;
    }
}
