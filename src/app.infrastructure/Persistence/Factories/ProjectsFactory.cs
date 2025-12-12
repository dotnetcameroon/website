using app.domain.Models.ProjectsAggregate;
using Bogus;
using EntityFrameworkCore.Seeder.Base;

namespace app.infrastructure.Persistence.Factories;

public class ProjectsFactory : Factory<Project>
{
    protected override Faker<Project> BuildRules()
    {
        return new Faker<Project>()
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Github, f => f.Internet.Url())
            .RuleFor(p => p.Website, f => f.Internet.Url())
            .RuleFor(p => p.AuthorHandle, f => f.Internet.UserName())
            .RuleFor(p => p.Technologies, f => string.Join(',', f.Lorem.Words(f.Random.Number(1, 5))));
    }
}
