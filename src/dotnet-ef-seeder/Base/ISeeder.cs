namespace EntityFrameworkCore.Seeder.Base;

public interface ISeeder
{
    Task SeedAsync();
}
