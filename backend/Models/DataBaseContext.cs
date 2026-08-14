using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
namespace whm.Models
{
    public class DataBaseContext:DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
    : base(options)
        {
        }
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=StockMangment;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.User_Email)
                .IsUnique();
        }
    }
}
