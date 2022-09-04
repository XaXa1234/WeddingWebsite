using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Models;

namespace WeddingWebsite.Db
{
    public class ApplicationDbContext : DbContext,  IApplicationDbContext
    {
        public DbSet<RsvpGuest> RsvpGuests { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        { }
    }
}
