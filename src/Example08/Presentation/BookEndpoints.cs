using Example08.Domain;
using Example08.Infrastructure.Repositories;

namespace Example08.Presentation;

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
    private readonly IGenericRepository _repository;
    private readonly ILogger<BookEndpoints> _logger;

    public BookEndpoints(IGenericRepository repository, ILogger<BookEndpoints> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await _repository.GetAllAsync<Book>(cancellationToken);
        return books;
    }

    public async Task<Book> GetBookByIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync<Book>(bookId, cancellationToken);
        return book;
    }
    
    public async Task<int> AddBookAsync(Book book, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(book, cancellationToken);
        var rows = await _repository.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken)
    {
        _repository.Update(book);
        var rows = await _repository.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> DeleteBookAsync(Book book, CancellationToken cancellationToken)
    {
        _repository.Delete(book);
        var rows = await _repository.SaveChangesAsync(cancellationToken);
        return rows;
    }
}