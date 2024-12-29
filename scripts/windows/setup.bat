# Spin up the database in detached mode
docker compose up -d

# Wait until the SqlServer container is ready
$containerName = "sqlserver"

while (-not (docker exec $containerName /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P $password -Q "SELECT 1" -ErrorAction SilentlyContinue)) {
    Write-Host "Waiting for SQL Server to be ready..."
    Start-Sleep -Seconds 3
}

# Migrate the database
dotnet ef database update -s ./app -p ./app.infrastructure

# Seed the database
dotnet run --project ./app/ --seed RolesSeeder PartnersSeeder EventsSeeder AdminsSeeder

# Shut down the database
docker compose down
