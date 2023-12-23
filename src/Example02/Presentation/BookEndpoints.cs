using Example02.Domain;
using Example02.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Example02.Presentation;

public interface IBookEndpoints
{
    Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken);
    Task<Book> GetBookByIdAsync(int bookId, CancellationToken cancellationToken);
    Task<int> AddBookAsync(Book book, CancellationToken cancellationToken);
    Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken);
    Task<int> DeleteBookAsync(Book book, CancellationToken cancellationToken);
}

public class BookEndpoints : IBookEndpoints
{
    private readonly BookDbContext _context;
    private readonly ILogger<BookEndpoints> _logger;

    public BookEndpoints(BookDbContext context, ILogger<BookEndpoints> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await _context.Books.ToListAsync(cancellationToken);
        return books;
    }

    public async Task<Book> GetBookByIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { bookId }, cancellationToken);
        return book;
    }
    
    public async Task<int> AddBookAsync(Book book, CancellationToken cancellationToken)
    {
        await _context.Books.AddAsync(book, cancellationToken);
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken)
    {
        _context.Entry(book).State = EntityState.Modified;
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> DeleteBookAsync(Book book, CancellationToken cancellationToken)
    {
        _context.Remove(book);
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }
}