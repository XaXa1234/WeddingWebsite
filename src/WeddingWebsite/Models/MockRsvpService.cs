using WeddingWebsite.Services;

namespace WeddingWebsite.Models
{
    public class MockRsvpService : IRsvpService
    {
        private RsvpGuest xaxa = new RsvpGuest() { Email="urtizverea@hotmail.fr"};
        public string DecodeRsvpEmail(string emailEncoded)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(emailEncoded);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public string EncodeRsvpEmail(string email)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(email);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public Task<RsvpGuest> FindRsvp(string email)
        {
            if(email == xaxa.Email)
                return Task.FromResult(xaxa);
            return Task.FromResult(default(RsvpGuest));
        }

        public async Task UpdateRsvp(string email, string firstName, string lastName, string phoneNumber, bool? isComing, string comment, bool? hasGuest,
                                string guestFirstName, string guestlastName)
        {
            var guest = await FindRsvp(email);
            if(guest == null) throw new Exception("Cannot find the guest");
            guest.FirsName = firstName;
            guest.LastName = lastName;
            guest.PhoneNumber = phoneNumber;
            guest.IsComing = isComing;
            guest.Comment = comment;
            guest.HasGuest = hasGuest;
            guest.GuestFirstName = guestFirstName;
            guest.GuestLastName = guestlastName;
        }
    }
}
