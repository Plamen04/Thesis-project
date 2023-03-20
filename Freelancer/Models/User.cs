using Microsoft.AspNetCore.Identity;

namespace Freelancer.Models
{
    public class User:IdentityUser
    {
        public string? Name { get; set; }
    }
}
