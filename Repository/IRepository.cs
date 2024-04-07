using Microsoft.EntityFrameworkCore;

namespace MovieCatalog.Repository;

public interface IRepository<T> where T : class{
    public DbSet<T>? GetAll();
    public Task<T?> FindAsync(int id);
    public Task InsertAsync(T obj);
    public Task UpdateAsync(T obj);
    public Task RemoveAsync(int id);
    public Task<bool> AnyAsync();
    public Task<bool> AnyAsync(int id);
}