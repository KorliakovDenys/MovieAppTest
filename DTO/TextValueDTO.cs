using System.Net.Mime;

namespace MovieCatalog.DTO;

public class TextValueDTO(string text, int value) {
    public string Text { get; set; } = text;

    public int Value { get; set; } = value;
}