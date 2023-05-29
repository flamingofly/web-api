using Microsoft.EntityFrameworkCore;//inherited when we had download nuget package of in memory
using my_first_webapi.Models;

namespace my_first_webapi.Data
{
    public class ContactsAPIDbContext : DbContext
    {
        public ContactsAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
