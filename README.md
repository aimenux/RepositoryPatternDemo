[![.NET](https://github.com/aimenux/RepositoryPatternDemo/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/aimenux/RepositoryPatternDemo/actions/workflows/ci.yml)

# RepositoryPatternDemo
```
Using repository pattern with entity framework in web api projects
```

In this repo, i m exploring various ways of using repository pattern in web api projects
>
> :one: `Example01` use controller api with directly the db context
>
> :two: `Example02` use minimal api with directly the db context
>
> :three: `Example03` use controller api with specific repository
>
> :four: `Example04` use minimal api with specific repository
>
> :five: `Example05` use controller api with generic repository
>
> :six: `Example06` use minimal api with generic repository
> 
> :seven: `Example07` use controller api with generic repository & specific unit of work
>
> :eight: `Example08` use minimal api with generic repository & specific unit of work
>
> :nine: `Example09` use controller api with generic repository & generic unit of work
>
> 🔟 `Example10` use minimal api with generic repository & generic unit of work
> 
> In order to setup the database, follow these steps :
> - Run this docker command : `docker run -e ‘ACCEPT_EULA=Y’ -e ‘SA_PASSWORD=Pa55w0rd’ -p 1433:1433 -d mcr.microsoft.com/mssql/server`
> - Use this connection string : `"Data Source=localhost;Initial Catalog=BooksDB;User Id=sa;Password=Pa55w0rd;"`
> - Run database migrations : `dotnet-ef database update`
> 

**`Tools`** : net 6.0, ef-core, xunit, fluent-assertions 