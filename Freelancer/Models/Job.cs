using System.ComponentModel.DataAnnotations;

namespace Freelancer.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int JobTypeId { get; set; }
        public JobType? JobType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
