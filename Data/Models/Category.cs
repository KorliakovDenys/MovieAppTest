using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalog.Data.Models;

public class Category {
    private int? _parentCategoryId;
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    public int? ParentCategoryId {
        get => _parentCategoryId;
        set {
            if (value > 0) {
                _parentCategoryId = value;
            }
        }
    }

    [ForeignKey("ParentCategoryId")]
    public Category? ParentCategory { get; set; }

    public ICollection<Category>? SubCategories { get; set; }

    public ICollection<FilmCategory>? FilmCategories { get; set; }
}