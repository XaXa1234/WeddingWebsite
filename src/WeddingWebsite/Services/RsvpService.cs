using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Db;
using WeddingWebsite.Models;

namespace WeddingWebsite.Services
{
    public class RsvpService : IRsvpService
    {
        private readonly IApplicationDbContext dbContext;

        public RsvpService(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

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

        public async Task<RsvpGuest?> FindRsvp(string? email)
        {
            return await dbContext.RsvpGuests.FirstOrDefaultAsync(r => r.Email == email);
        }

        public async Task IsComing(RsvpGuest guest)
        {
            var g = await FindRsvp(guest?.Email);
            if (g == null) throw new Exception("Cannot find the guest");
            guest.IsComing = true;
            await dbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task IsNotComing(RsvpGuest guest)
        {
            var g = await FindRsvp(guest?.Email);
            if (g == null) throw new Exception("Cannot find the guest");
            guest.IsComing = false;
            await dbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task UpdateRsvp(string email, string firstName, string lastName, string phoneNumber, bool? isComing, string comment, bool? hasGuest, string guestFirstName, string guestlastName)
        {
            var g = await FindRsvp(email);
            if (g == null) throw new Exception("Cannot find the guest");
            g.FirsName = firstName;
            g.LastName = lastName;
            g.PhoneNumber = phoneNumber;
            g.IsComing = isComing;
            g.Comment = comment;
            g.HasGuest = hasGuest;
            g.GuestFirstName = guestFirstName;
            g.GuestLastName = guestlastName;
            await dbContext.SaveChangesAsync(new CancellationToken());
        }
    }
}
