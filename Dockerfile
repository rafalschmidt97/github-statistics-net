FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS Base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Restore dotnet before build to allow for caching
WORKDIR /
COPY StyleCop.json ./
COPY CodeAnalysis.ruleset ./
COPY CodeAnalysis.targets ./
COPY src/GithubStatistics.WebAPI/GithubStatistics.WebAPI.csproj /src/GithubStatistics.WebAPI/
COPY src/GithubStatistics.Application/GithubStatistics.Application.csproj /src/GithubStatistics.Application/
COPY src/GithubStatistics.Common/GithubStatistics.Common.csproj /src/GithubStatistics.Common/
COPY src/GithubStatistics.Infrastructure/GithubStatistics.Infrastructure.csproj /src/GithubStatistics.Infrastructure/

RUN dotnet restore /src/GithubStatistics.WebAPI/GithubStatistics.WebAPI.csproj

# Copy source files and build
COPY . ./

RUN dotnet build /src/GithubStatistics.WebAPI/GithubStatistics.WebAPI.csproj --no-restore -c Release
RUN dotnet publish /src/GithubStatistics.WebAPI/GithubStatistics.WebAPI.csproj --no-restore -c Release -o /app

# Copy compiled app to the runtime container
FROM base AS final
COPY --from=build /app .
ENTRYPOINT ["dotnet", "GithubStatistics.WebAPI.dll"]
