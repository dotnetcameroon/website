# Spin up the database in detached mode
docker compose up -d

# Wait until the SQL Server container is healthy
until [ "$(docker inspect --format='{{.State.Health.Status}}' dotnetcameroon.sqlserver)" = "healthy" ]; do
  echo "Waiting for SQL Server to be healthy..."
  sleep 3
done

# Migrate the database
dotnet ef database update -s ./src//app -p ./src/app.infrastructure --context AppDbContext && \

# Seed the database
dotnet run --project ./src/app/ --seed RolesSeeder PartnersSeeder EventsSeeder AdminsSeeder ProjectsSeeder

# Shut down the database
docker compose down
