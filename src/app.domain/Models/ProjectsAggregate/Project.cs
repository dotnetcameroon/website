namespace app.domain.Models.ProjectsAggregate;

public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AuthorHandle { get; set; } = string.Empty;
    public string Technologies { get; set; } = string.Empty; // comma separated list
    public string? Github { get; set; }
    public string? Website { get; set; }
}
