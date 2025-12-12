using System.Text.Json;
using app.business.Services;
using app.domain.Models.ProjectsAggregate;
using app.shared.Utilities;

namespace app.Api.Projects;

public static class ProjectsApi
{
    public static IEndpointRouteBuilder MapProjectsApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects", () => "Hello Projects!")
            .RequireAuthorization(Policies.JwtAuthOnly);

        endpoints.MapPost("/api/projects", async (
            IFormFile formFile, IProjectService projectService) =>
        {
            using var reader = new StreamReader(formFile.OpenReadStream());
            var content = await reader.ReadToEndAsync();
            var projects = JsonSerializer.Deserialize<ProjectDto[]>(content) ?? throw new InvalidOperationException("Invalid JSON");
            await projectService.RefreshAsync(projects.Select(p => new Project
            {
                Id = Guid.NewGuid(),
                Title = p.Title,
                Description = p.Description,
                AuthorHandle = p.AuthorHandle,
                Technologies = p.Technologies,
                Github = p.Github,
                Website = p.Website
            }));
            return Results.Ok();
        })
            .RequireAuthorization(Policies.JwtAuthOnly);

        endpoints.MapPost("/api/projects/json", async (PushDataRequest request, IProjectService projectService) =>
        {
            var projects = JsonSerializer.Deserialize<ProjectDto[]>(request.Json) ?? throw new InvalidOperationException("Invalid JSON");
            await projectService.RefreshAsync(projects.Select(p => new Project
            {
                Id = Guid.NewGuid(),
                Title = p.Title,
                Description = p.Description,
                AuthorHandle = p.AuthorHandle,
                Technologies = p.Technologies,
                Github = p.Github,
                Website = p.Website
            }));
            return Results.Ok();
        })
            .RequireAuthorization(Policies.JwtAuthOnly);

        return endpoints;
    }
}

public record struct ProjectDto(
    string Title,
    string Description,
    string AuthorHandle,
    string Technologies,
    string? Github,
    string? Website);

public record PushDataRequest(
    string Json
);
