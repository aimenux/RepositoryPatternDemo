# RepositoryPatternDemo
Using repository pattern with entity framework in web api projects

docker run -e ‘ACCEPT_EULA=Y’ -e ‘SA_PASSWORD=Pa55w0rd’ -p 1433:1433 -d mcr.microsoft.com/mssql/server

dotnet ef migrations add InitialCreate -o .\Infrastructure\Migrations
dotnet-ef migrations remove
dotnet-ef database drop
dotnet-ef database update