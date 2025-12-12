namespace app.domain.ViewModels;

public record Sponsor(
    string Name,
    string LogoUrl,
    string Description,
    string? WebsiteUrl = null
);
