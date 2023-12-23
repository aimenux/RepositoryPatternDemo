using System.Collections.Concurrent;
using Example09.Infrastructure.Repositories;

namespace Example09.Infrastructure;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly BookDbContext _context;
    private readonly IDictionary<Type, object> _repositories;
    
    public UnitOfWork(BookDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _repositories = new ConcurrentDictionary<Type, object>();
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.TryGetValue(typeof(TEntity), out var cachedRepository))
        {
            return cachedRepository as IGenericRepository<TEntity>;
        }
        
        var repository = new GenericRepository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }
}