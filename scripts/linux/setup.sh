# Spin up the database in detached mode
docker compose up -d

# Wait until the SQL Server container is ready
until docker exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '!Passw0rd' -Q "SELECT 1" &> /dev/null; do
  echo "Waiting for SQL Server to be ready..."
  sleep 3
done

# Migrate the database
dotnet ef database update --project ./app/ && \

# Seed the database
dotnet run --project ./app/ --seed RolesSeeder
dotnet run --project ./app/ --seed PartnersSeeder
dotnet run --project ./app/ --seed EventsSeeder
dotnet run --project ./app/ --seed AdminsSeeder
