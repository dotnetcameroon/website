# Spin up the database in detached mode
docker-compose up -d

# Wait until the PostgreSQL container is ready
$containerName = "postgres"

while (-not (docker exec $containerName pg_isready -U admin -h localhost -q)) {
    Write-Host "Waiting for PostgreSQL to be ready..."
    Start-Sleep -Seconds 3
}

# Migrate the database
dotnet ef database update --project ./app/ 

# Seed the database
dotnet run --project ./app/ --seed RolesSeeder
dotnet run --project ./app/ --seed PartnersSeeder
dotnet run --project ./app/ --seed EventsSeeder
dotnet run --project ./app/ --seed AdminsSeeder
