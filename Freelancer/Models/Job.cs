using System.ComponentModel.DataAnnotations;

namespace Freelancer.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
    }
}
