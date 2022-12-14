namespace WeddingWebsite.Models

{
    public class RsvpGuest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? FirsName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsComing { get; set; }
        public string? Comment { get; set; }
        public bool CanHaveGuest { get; set; }
        public bool? HasGuest { get; set; }
        public string? GuestFirstName { get; set; }
        public string? GuestLastName { get; set; }
        public string? CommentGuest { get; set; }
    }
}
