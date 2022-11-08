using WeddingWebsite.Models;

namespace WeddingWebsite.Services
{
    public class MockRsvpService : IRsvpService
    {
        private RsvpGuest? xaxa = new RsvpGuest() { Email = "urtizverea@hotmail.fr", CanHaveGuest = true };
        public string DecodeRsvpEmail(string emailEncoded)
        {
            var base64EncodedBytes = Convert.FromBase64String(emailEncoded);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public string EncodeRsvpEmail(string email)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(email);
            return Convert.ToBase64String(plainTextBytes);
        }

        public Task<RsvpGuest?> FindRsvp(string? email)
        {
            if (email == xaxa?.Email)
                return Task.FromResult(xaxa);
            return Task.FromResult(default(RsvpGuest));
        }

        public async Task IsComing(string? email)
        {
            var guest = await FindRsvp(email);
            await IsComing(guest);
        }

        public Task IsComing(RsvpGuest? guest)
        {
            if (guest == null) throw new Exception("Cannot find the guest");
            guest.IsComing = true;
            return Task.CompletedTask;
        }

        public async Task IsNotComing(string? email)
        {
            var guest = await FindRsvp(email);
           await IsComing(guest);
        }

        public Task IsNotComing(RsvpGuest guest)
        {
            if (guest == null) throw new Exception("Cannot find the guest");
            guest.IsComing = false;
            return Task.CompletedTask;
        }

        public async Task UpdateRsvp(string email, string firstName, string lastName, string address, string phoneNumber, string comment, bool? hasGuest,
                                string guestFirstName, string guestlastName, string commentGuest)
        {
            var guest = await FindRsvp(email);
            if (guest == null) throw new Exception("Cannot find the guest");
            guest.FirsName = firstName;
            guest.LastName = lastName;
            guest.PhoneNumber = phoneNumber;
            guest.Comment = comment;
            guest.HasGuest = hasGuest;
            guest.GuestFirstName = guestFirstName;
            guest.GuestLastName = guestlastName;
        }
    }
}
