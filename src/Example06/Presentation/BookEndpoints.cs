using Example06.Domain;
using Example06.Infrastructure.Repositories;

namespace Example06.Presentation;

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
    private readonly IGenericRepository<Book> _repository;
    private readonly ILogger<BookEndpoints> _logger;

    public BookEndpoints(IGenericRepository<Book> repository, ILogger<BookEndpoints> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await _repository.GetAllAsync(cancellationToken);
        return books;
    }

    public async Task<Book> GetBookByIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(bookId, cancellationToken);
        return book;
    }
    
    public async Task<int> AddBookAsync(Book book, CancellationToken cancellationToken)
    {
        var rows = await _repository.AddAsync(book, cancellationToken);
        return rows;
    }

    public async Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken)
    {
        var rows = await _repository.UpdateAsync(book, cancellationToken);
        return rows;
    }

    public async Task<int> DeleteBookAsync(Book book, CancellationToken cancellationToken)
    {
        var rows = await _repository.DeleteAsync(book, cancellationToken);
        return rows;
    }
}