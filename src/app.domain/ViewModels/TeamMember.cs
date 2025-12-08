namespace app.domain.ViewModels;

public record TeamMember(
    string Name,
    string Role,
    string ImageUrl,
    string Description,
    string? LinkedIn = null,
    string? Website = null,
    string? Github = null
);
