using Microsoft.EntityFrameworkCore;

namespace Example08.Infrastructure.Repositories;

public interface IGenericRepository
{
    Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(CancellationToken cancellationToken) where TEntity : class;
    Task<TEntity> GetByIdAsync<TEntity>(int id, CancellationToken cancellationToken) where TEntity : class;
    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class;
    void Update<TEntity>(TEntity entity) where TEntity : class;
    void Delete<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public class GenericRepository : IGenericRepository
{
    private readonly BookDbContext _context;

    public GenericRepository(BookDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(CancellationToken cancellationToken) where TEntity : class
    {
        var items = await _context.Set<TEntity>().ToListAsync(cancellationToken);
        return items;
    }

    public async Task<TEntity> GetByIdAsync<TEntity>(int id, CancellationToken cancellationToken) where TEntity : class
    {
        var item = await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        return item;
    }

    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update<TEntity>(TEntity entity) where TEntity : class
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }
}