using EntityFrameworkConsoleSample.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EntityFrameworkConsoleSample.Data;

public class LibraryContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<HistoricalPeriod> HistoricalPeriods { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<AuthorBook>()
            .HasOne(e => e.Author)
            .WithMany(e => e.AuthorBooks)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<AuthorBook>()
            .HasOne(e => e.Author)
            .WithMany(e => e.AuthorBooks)
            .OnDelete(DeleteBehavior.Restrict);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(@"Server=DESKTOP-JPGDIHT;Database=TestLibraryDb;trusted_connection=true;encrypt=false");
        
    }
}