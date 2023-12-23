[![.NET](https://github.com/aimenux/RepositoryPatternDemo/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/aimenux/RepositoryPatternDemo/actions/workflows/ci.yml)

# RepositoryPatternDemo
```
Using repository pattern with entity framework in web api projects
```

In this repo, i m exploring various ways of using repository pattern in web api projects
>
> :heavy_minus_sign: `Example01` use controller api with directly the db context
>
> :heavy_minus_sign: `Example02` use minimal api with directly the db context
>
> :heavy_minus_sign: `Example03` use controller api with specific repository
>
> :heavy_minus_sign: `Example04` use minimal api with specific repository
>
> :heavy_minus_sign: `Example05` use controller api with generic repository
>
> :heavy_minus_sign: `Example06` use minimal api with generic repository
> 
> :heavy_minus_sign: `Example07` use controller api with generic repository & specific unit of work
>
> :heavy_minus_sign: `Example08` use minimal api with generic repository & specific unit of work
>
> :heavy_minus_sign: `Example09` use controller api with generic repository & generic unit of work
>
> :heavy_minus_sign: `Example10` use minimal api with generic repository & generic unit of work
> 
> In order to setup the database, follow these steps for some example :
> - Run this docker command : `docker run -e ‘ACCEPT_EULA=Y’ -e ‘SA_PASSWORD=Pa55w0rd’ -p 1433:1433 -d mcr.microsoft.com/mssql/server`
> - Use this connection string : `"Data Source=localhost;Initial Catalog=BooksDB;User Id=sa;Password=Pa55w0rd;TrustServerCertificate=True;"`
> - Run database migrations : `dotnet-ef database update`
> 

**`Tools`** : net 7.0, ef-core, xunit, fluent-assertions 