using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NewsAppClasses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NewsAPI.Data.Contexts
{
    public class MainDBContext : IdentityDbContext<User>
    {
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<News> News => Set<News>();

        public MainDBContext(DbContextOptions<MainDBContext> options):base(options)
        {

        }
    }
}
