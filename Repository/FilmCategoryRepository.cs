using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Data.Models;

namespace MovieCatalog.Repository;

public class FilmCategoryRepository(MovieDbContext context) : IRepository<FilmCategory> {
    public DbSet<FilmCategory>? GetAll() {
        return context.FilmCategories;
    }

    public async Task<FilmCategory?> FindAsync(int id) {
        if (context.FilmCategories != null) {
            return await context.FilmCategories.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);
        }

        return null;
    }

    public async Task InsertAsync(FilmCategory obj) {
        if (context.FilmCategories != null) {
            await context.FilmCategories.AddAsync(obj);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(FilmCategory obj) {
        context.FilmCategories?.Update(obj);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id) {
        var film = await FindAsync(id);
        if (film != null) {
            context.FilmCategories?.Remove(film);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> AnyAsync() {
        return context.FilmCategories != null && await context.FilmCategories.AnyAsync();
    }
    
    public async Task<bool> AnyAsync(int id) {
        return context.FilmCategories != null && await context.FilmCategories.Where(f => f.Id == id).AnyAsync();
    }
}