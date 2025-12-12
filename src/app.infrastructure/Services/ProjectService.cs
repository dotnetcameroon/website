using app.business.Services;
using app.domain.Models.ProjectsAggregate;
using app.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace app.infrastructure.Services;

public class ProjectService(ProjectDbContext context) : IProjectService
{
    public Task<Project[]> GetAllAsync()
    {
        return context.Projects.ToArrayAsync();
    }

    public async Task RefreshAsync(IEnumerable<Project> projects)
    {
        await context.Projects.ExecuteDeleteAsync();
        await context.Projects.AddRangeAsync(projects);
        await context.SaveChangesAsync();
    }
}
