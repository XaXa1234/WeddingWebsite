using WeddingWebsite.Models;

namespace WeddingWebsite.Services
{
    public interface IRsvpService
    {
        string EncodeRsvpEmail(string email);
        string DecodeRsvpEmail(string emailEncoded);
        Task<RsvpGuest?> FindRsvp(string? email);
        Task UpdateRsvp(string email, string firstName, string lastName, string address, string phoneNumber, string comment, bool? hasGuest,
                                string guestFirstName, string guestlastName, string commentGuest);
        Task IsNotComing(RsvpGuest guest);
        Task IsComing(RsvpGuest guest);

    }
}
