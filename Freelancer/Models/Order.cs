namespace Freelancer.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job? Jobs { get; set; }
        public string? UserId { get; set; }
        public User? Users { get; set; }
        public bool IsApproved { get; set; }
    }
}
