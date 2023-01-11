namespace EventBookerBack.ViewModels
{
    public class EventVM
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Tickets { get; set; }
        public int LocationId { get; set; }
        public string UserEmail { get; set; }

    }
}
