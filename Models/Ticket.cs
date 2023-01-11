namespace EventBookerBack.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }    
    }
}
