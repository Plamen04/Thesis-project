namespace Freelancer.Models
{
    public class JobType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ICollection<Job>? Jobs { get; set;}
    }
}
