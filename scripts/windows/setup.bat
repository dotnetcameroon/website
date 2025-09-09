# Spin up the database in detached mode
docker compose up -d

# Wait until the SqlServer container is healthy
$containerName = "dotnetcameroon.sqlserver"

while ((docker inspect --format='{{.State.Health.Status}}' $containerName) -ne "healthy") {
    Write-Host "Waiting for SQL Server to be healthy..."
    Start-Sleep -Seconds 3
}

# Migrate the database
dotnet ef database update -s ./src/app -p ./src/app.infrastructure --context AppDbContext

# Seed the database
dotnet run --project ./src/app/ --seed RolesSeeder PartnersSeeder EventsSeeder AdminsSeeder ProjectsSeeder

# Shut down the database
docker compose down
