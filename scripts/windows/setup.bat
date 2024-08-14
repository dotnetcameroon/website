# Spin up the database in detached mode
docker-compose up -d

# Wait until the SQL Server container is ready
$containerName = "sqlserver"
$password = "!Passw0rd"

while (-not (docker exec $containerName /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -Q "SELECT 1" -ErrorAction SilentlyContinue)) {
    Write-Host "Waiting for SQL Server to be ready..."
    Start-Sleep -Seconds 3
}

# Migrate the database
dotnet ef database update --project ./app/ 

# Seed the database
dotnet run --project ./app/ --seed RolesSeeder
dotnet run --project ./app/ --seed PartnersSeeder
dotnet run --project ./app/ --seed EventsSeeder
