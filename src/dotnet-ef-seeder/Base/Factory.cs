using Bogus;

namespace EntityFrameworkCore.Seeder.Base;
public abstract class Factory<TEntity>
    where TEntity : class
{
    protected Faker<TEntity> Faker => BuildRules();
    protected abstract Faker<TEntity> BuildRules();
    public virtual TEntity Generate()
    {
        return Faker.Generate();
    }

    public virtual TEntity[] Generate(int number)
    {
        return Faker.Generate(number).ToArray();
    }
}
