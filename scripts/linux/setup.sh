# Spin up the database in detached mode
docker compose up -d

# Wait until the SQL Server container is ready
until docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P '@Passw0rd' -Q "SELECT 1" -C &> /dev/null; do
  echo "Waiting for SQL Server to be ready..."
  sleep 3
done

# Migrate the database
dotnet ef database update -s ./src//app -p ./src/app.infrastructure --context AppDbContext && \

# Seed the database
dotnet run --project ./src/app/ --seed RolesSeeder PartnersSeeder EventsSeeder AdminsSeeder ProjectsSeeder

# Shut down the database
docker compose down
