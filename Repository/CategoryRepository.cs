using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Data.Models;

namespace MovieCatalog.Repository;

public class CategoryRepository(MovieDbContext context) : IRepository<Category> {
    public DbSet<Category>? GetAll() {
        return context.Categories;
    }

    public async Task<Category?> FindAsync(int id) {
        if (context.Categories != null) {
            return await context.Categories.Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);
        }

        return null;
    }

    public async Task InsertAsync(Category obj) {
        if (context.Categories != null) {
            await context.Categories.AddAsync(obj);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Category obj) {
        context.Categories?.Update(obj);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id) {
        var film = await FindAsync(id);
        if (film != null) {
            context.Categories?.Remove(film);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> AnyAsync() {
        return context.Categories != null && await context.Categories.AnyAsync();
    }
    
    public async Task<bool> AnyAsync(int id) {
        return context.Categories != null && await context.Categories.Where(f => f.Id == id).AnyAsync();
    }
}