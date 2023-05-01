FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApiBenchmark.BenchmarkTests/ApiBenchmark.BenchmarkTests.csproj", "ApiBenchmark.BenchmarkTests/"]
RUN dotnet restore "ApiBenchmark.BenchmarkTests/ApiBenchmark.BenchmarkTests.csproj"
COPY . .
WORKDIR "/src/ApiBenchmark.BenchmarkTests"
RUN dotnet build "ApiBenchmark.BenchmarkTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiBenchmark.BenchmarkTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiBenchmark.BenchmarkTests.dll"]
