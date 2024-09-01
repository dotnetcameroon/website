# Spin up the database in detached mode
docker compose -f docker-compose.postgres.yml up -d

# Wait until the PostgreSQL container is ready
until docker exec postgres pg_isready -U admin &> /dev/null; do
  echo "Waiting for PostgreSQL to be ready..."
  sleep 3
done

# Migrate the database
dotnet ef database update --project ./app/ && \

# Seed the database
dotnet run --project ./app/ --seed RolesSeeder PartnersSeeder EventsSeeder AdminsSeeder

# Shut down the database
docker compose -f docker-compose.postgres.yml down
