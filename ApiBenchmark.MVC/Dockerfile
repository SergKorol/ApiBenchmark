FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends ca-certificates curl software-properties-common 
RUN curl https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -o packages-microsoft-prod.deb && dpkg -i packages-microsoft-prod.deb && rm packages-microsoft-prod.deb
RUN apt-get update && apt-get install -y --no-install-recommends dotnet-sdk-6.0
RUN apt-get update && apt-get install -y --no-install-recommends dotnet-sdk-7.0
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["src/ApiBenchmark/ApiBenchmark.MVC/ApiBenchmark.MVC.csproj", "src/ApiBenchmark/ApiBenchmark.MVC/ApiBenchmark.MVC.csproj"]
COPY ["src/ApiBenchmark/ApiBenchmark.Services/ApiBenchmark.Services.csproj", "src/ApiBenchmark/ApiBenchmark.Services/ApiBenchmark.Services.csproj"]
COPY ["src/ApiBenchmark/ApiBenchmark.Application/ApiBenchmark.Application.csproj", "src/ApiBenchmark/ApiBenchmark.Application/ApiBenchmark.Application.csproj"]
COPY ["src/ApiBenchmark/ApiBenchmark.BenchmarkTests/ApiBenchmark.BenchmarkTests.csproj", "src/ApiBenchmark/ApiBenchmark.BenchmarkTests/ApiBenchmark.BenchmarkTests.csproj"]
COPY ["src/ApiBenchmark/ApiBenchmark.UnitTests/ApiBenchmark.UnitTests.csproj", "src/ApiBenchmark/ApiBenchmark.UnitTests/ApiBenchmark.UnitTests.csproj"]
COPY ["NuGet.config", "./"]
COPY ["src/ApiBenchmark/ApiBenchmark.sln", "src/ApiBenchmark/ApiBenchmark.sln"]
RUN dotnet restore "src/ApiBenchmark/ApiBenchmark.sln"
COPY . .
WORKDIR "src/ApiBenchmark/ApiBenchmark.MVC"
RUN dotnet publish -c Release -f net7.0 -o /app

FROM build AS application
WORKDIR "src/ApiBenchmark/ApiBenchmark.App"

#COPY . .
FROM build AS benchmark
WORKDIR "src/ApiBenchmark/ApiBenchmark.BenchmarkTests"

FROM build AS benchmark
WORKDIR "src/ApiBenchmark/ApiBenchmark.UnitTests"

FROM build AS services
WORKDIR "src/ApiBenchmark/ApiBenchmark.Services"

FROM build AS publish
RUN dotnet publish "ApiBenchmark.MVC.csproj" -c Release -f net7.0 -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY . .
ENTRYPOINT ["dotnet", "ApiBenchmark.MVC.dll"]
