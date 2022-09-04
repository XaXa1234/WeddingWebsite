using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Models;

namespace WeddingWebsite.Db
{
    public interface IApplicationDbContext
    {
        DbSet<RsvpGuest> RsvpGuests { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
