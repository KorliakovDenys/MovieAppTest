using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Data.Models;

namespace MovieCatalog.Repository;

public class FilmRepository(MovieDbContext context) : IRepository<Film> {
    public DbSet<Film>? GetAll() {
        return context.Films;
    }

    public async Task<Film?> FindAsync(int id) {
        if (context.Films != null) {
            return await context.Films.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);
        }

        return null;
    }

    public async Task InsertAsync(Film obj) {
        if (context.Films != null) {
            await context.Films.AddAsync(obj);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Film obj) {
        context.Films?.Update(obj);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id) {
        var film = await FindAsync(id);
        if (film != null) {
            context.Films?.Remove(film);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> AnyAsync() {
        return context.Films != null && await context.Films.AnyAsync();
    }

    public async Task<bool> AnyAsync(int id) {
        return context.Films != null && await context.Films.Where(f => f.Id == id).AnyAsync();
    }
}