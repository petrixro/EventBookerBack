namespace EventBookerBack.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Tickets { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }

        public Location Location { get; set; }
        public User UserAdmin { get; set; }

    }

    
}
