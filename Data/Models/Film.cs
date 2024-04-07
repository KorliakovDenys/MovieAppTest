using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Data.Models;

public class Film {
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [StringLength(200)]
    public string Director { get; set; }

    public DateTime ReleaseDate { get; set; }

    public ICollection<FilmCategory>? FilmCategories { get; set; }
}