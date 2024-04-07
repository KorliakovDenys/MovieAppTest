using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalog.Data.Models;

public class FilmCategory {
    [Key]
    public int Id { get; set; }

    [Required]
    public int FilmId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey("FilmId")]
    public Film Film { get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}