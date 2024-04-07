using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Models;

namespace MovieCatalog.Data;

public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options) {
    public DbSet<Film>? Films { get; set; }
    public DbSet<Category>? Categories { get; set; }
    public DbSet<FilmCategory>? FilmCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Film>()
            .Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Self-referencing relationship
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.NoAction); // to avoid cascade delete issue

        modelBuilder.Entity<FilmCategory>()
            .HasOne(fc => fc.Film)
            .WithMany(f => f.FilmCategories)
            .HasForeignKey(fc => fc.FilmId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FilmCategory>()
            .HasOne(fc => fc.Category)
            .WithMany(c => c.FilmCategories)
            .HasForeignKey(fc => fc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    private bool RemoveCategory(Category category) {
        using var transaction = Database.BeginTransaction();
        try {
            if (category.SubCategories != null && category.SubCategories.Any()) {
                category.SubCategories.Clear();
            }
            Remove(category);
            SaveChanges();

            transaction.Commit();
        } catch (Exception ex) {
            transaction.Rollback();
            return false;
        }

        return true;
    }
}