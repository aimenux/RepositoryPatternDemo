using Microsoft.EntityFrameworkCore;

namespace Example05.Infrastructure.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly BookDbContext _context;

    public GenericRepository(BookDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = await _context.Set<TEntity>().ToListAsync(cancellationToken);
        return items;
    }

    public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        return item;
    }

    public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Entry(entity).State = EntityState.Modified;
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }

    public async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Remove(entity);
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows;
    }
}