using Example04.Domain;
using Microsoft.EntityFrameworkCore;

namespace Example04.Infrastructure;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }
    
    public DbSet<Book> Books { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Book>()
            .HasIndex(x => x.Title);

        modelBuilder
            .Entity<Book>()
            .Property(x => x.Title)
            .HasMaxLength(100);
        
        modelBuilder
            .Entity<Book>()
            .HasIndex(x => x.Author);
        
        modelBuilder
            .Entity<Book>()
            .Property(x => x.Author)
            .HasMaxLength(100);

        modelBuilder
            .Entity<Book>()
            .HasData(GetBooks());
    }
    
    private static Book[] GetBooks()
    {
        return new[]
        {
            new Book(1, "Blue Remembered Earth", "Alastair Reynolds"),
            new Book(2, "The Glory and the Dream", "William Manchester")
        };
    }
}