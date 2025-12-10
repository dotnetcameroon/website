namespace app.domain.ViewModels;

public record Speaker(
    string Name,
    string Occupation,
    string ImageUrl,
    string? LinkedIn,
    string? Twitter,
    string? Github,
    string? Website = null,
    string Description = ""
);
