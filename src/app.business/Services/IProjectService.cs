using app.domain.Models.ProjectsAggregate;

namespace app.business.Services;

public interface IProjectService
{
    Task<Project[]> GetAllAsync();
    Task RefreshAsync(IEnumerable<Project> projects);
}
