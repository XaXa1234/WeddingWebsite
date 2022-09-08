namespace WeddingWebsite.Models
{
    public class PlaceToSleep
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? PostCode { get; set; }
        public string? Adress { get; set; }
        public string? City { get; set; }
        public decimal? lat { get; set; }
        public decimal? lon { get; set; }
    }
}
