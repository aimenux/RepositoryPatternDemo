using Example07.Domain;
using Example07.Infrastructure.Repositories;

namespace Example07.Infrastructure;

public interface IUnitOfWork
{
    IGenericRepository<Book> Books { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly BookDbContext _context;
    
    public UnitOfWork(BookDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Books = new GenericRepository<Book>(_context);
    }

    public IGenericRepository<Book> Books { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }
}