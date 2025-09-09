# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# copy central package management files first
COPY ["src/Directory.Build.props", "source/"]
COPY ["src/Directory.Packages.props", "source/"]

# copy csproj and restore as distinct layers
COPY ["src/dotnet-ef-seeder/dotnet-ef-seeder.csproj", "source/dotnet-ef-seeder/"]
COPY ["src/app.shared/app.shared.csproj", "source/app.shared/"]
COPY ["src/app.business/app.business.csproj", "source/app.business/"]
COPY ["src/app.client/app.client.csproj", "source/app.client/"]
COPY ["src/app.domain/app.domain.csproj", "source/app.domain/"]
COPY ["src/app.infrastructure/app.infrastructure.csproj", "source/app.infrastructure/"]
COPY ["src/app/app.csproj", "source/app/"]

RUN dotnet restore "source/app/app.csproj"

COPY ./src .
WORKDIR "/source/app/"
RUN dotnet build "app.csproj" -c Release -o /app/build

# Build the application for publishing
FROM build AS publish
RUN dotnet publish "app.csproj" -c Release -o /app/publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
EXPOSE 8080

# Copy the published output from the 'publish' stage
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "app.dll"]