using Example10.Domain;
using Example10.Infrastructure;

namespace Example10.Presentation;

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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BookEndpoints> _logger;

    public BookEndpoints(IUnitOfWork unitOfWork, ILogger<BookEndpoints> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await _unitOfWork.GetRepository<Book>().GetAllAsync(cancellationToken);
        return books;
    }

    public async Task<Book> GetBookByIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.GetRepository<Book>().GetByIdAsync(bookId, cancellationToken);
        return book;
    }
    
    public async Task<int> AddBookAsync(Book book, CancellationToken cancellationToken)
    {
        await _unitOfWork.GetRepository<Book>().AddAsync(book, cancellationToken);
        var rows = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken)
    {
        _unitOfWork.GetRepository<Book>().Update(book);
        var rows = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> DeleteBookAsync(Book book, CancellationToken cancellationToken)
    {
        _unitOfWork.GetRepository<Book>().Delete(book);
        var rows = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return rows;
    }
}