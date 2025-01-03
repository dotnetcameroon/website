using System.Text.Json;
using app.domain.Models.ProjectsAggregate;
using app.infrastructure.Persistence;
using app.shared.Utilities;
using Microsoft.EntityFrameworkCore;

namespace app.Api.Projects;
public static class ProjectsApi
{
    public static IEndpointRouteBuilder MapProjectsApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects", () => "Hello Projects!")
            .RequireAuthorization(Policies.JwtAuthOnly);

        endpoints.MapPost("/api/projects", async (IFormFile formFile, ProjectDbContext projectDb) =>
        {
            using var reader = new StreamReader(formFile.OpenReadStream());
            var content = await reader.ReadToEndAsync();
            var projects = JsonSerializer.Deserialize<ProjectDto[]>(content) ?? throw new InvalidOperationException("Invalid JSON");
            await RefreshProjectsDb(projectDb, projects);
            return Results.Ok();
        })
            .RequireAuthorization(Policies.JwtAuthOnly);

        endpoints.MapPost("/api/projects/json", async (string json, ProjectDbContext projectDb) =>
        {
            var projects = JsonSerializer.Deserialize<ProjectDto[]>(json) ?? throw new InvalidOperationException("Invalid JSON");
            await RefreshProjectsDb(projectDb, projects);
            return Results.Ok();
        })
            .RequireAuthorization(Policies.JwtAuthOnly);

        return endpoints;
    }

    private static async Task RefreshProjectsDb(ProjectDbContext projectDb, ProjectDto[] projects)
    {
        await projectDb.Projects.ExecuteDeleteAsync();
        await projectDb.Projects.AddRangeAsync(projects.Select(p => new Project
        {
            Id = Guid.NewGuid(),
            Title = p.Title,
            Description = p.Description,
            AuthorHandle = p.AuthorHandle,
            Technologies = p.Technologies,
            Github = p.Github,
            Website = p.Website
        }));
        await projectDb.SaveChangesAsync();
    }
}

public record struct ProjectDto(
    string Title,
    string Description,
    string AuthorHandle,
    string Technologies,
    string? Github,
    string? Website);
