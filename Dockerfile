# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80

COPY HealthApp.Razor/*.csproj ./HealthApp.Razor/
COPY HealthApp.Domain/*.csproj ./HealthApp.Domain/
COPY HealthApp.Tests/*.csproj ./HealthApp.Tests/
COPY *.sln ./
COPY wait-for-sql.sh .
RUN chmod +x wait-for-sql.sh

RUN dotnet restore
COPY . .

WORKDIR /app/HealthApp.Razor
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
COPY --from=build /app/wait-for-sql.sh .
RUN chmod +x wait-for-sql.sh

ENTRYPOINT ["./wait-for-sql.sh", "sqlserver", "dotnet", "HealthApp.Razor.dll"]
