using Example06;
using Example08.Domain;
using Example08.Infrastructure;
using Example08.Infrastructure.Repositories;
using Example08.Presentation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookEndpoints, BookEndpoints>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
if (!builder.Environment.IsContinuousIntegration())
{
    builder.Services.AddDbContext<BookDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("BooksDBConnection"), sqlServerOptions =>
        {
            sqlServerOptions.EnableRetryOnFailure();
        });
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app
    .MapGet("/api/books/list",
        async (IBookEndpoints endpoints, CancellationToken cancellationToken) =>
        {
            var books = await endpoints.GetBooksAsync(cancellationToken);
            return Results.Ok(books);
        })
    .WithName("GetBooks");

app
    .MapGet("/api/books/{bookId:int}",
        async (IBookEndpoints endpoints, int bookId, CancellationToken cancellationToken) =>
        {
            var book = await endpoints.GetBookByIdAsync(bookId, cancellationToken);
            return book is null ? Results.NotFound() : Results.Ok(book);
        })
    .WithName("GetBookById");

app
    .MapPost("/api/books",
        async (IBookEndpoints endpoints, Book book, CancellationToken cancellationToken) =>
        {
            await endpoints.AddBookAsync(book, cancellationToken);
            return Results.Created($"/api/books/{book.Id}", book);
        })
    .WithName("AddBook");

app
    .MapPut("/api/books/{bookId:int}",
        async (IBookEndpoints endpoints, int bookId, Book book, CancellationToken cancellationToken) =>
        {
            if (bookId != book.Id)
            {
                return Results.BadRequest();
            }

            var rows = await endpoints.UpdateBookAsync(book, cancellationToken);
            return rows <= 0 ? Results.NotFound() : Results.NoContent();
        })
    .WithName("UpdateBook");

app
    .MapDelete("/api/books/{bookId:int}",
        async (IBookEndpoints endpoints, int bookId, CancellationToken cancellationToken) =>
        {
            var book = await endpoints.GetBookByIdAsync(bookId, cancellationToken);
            if (book is null)
            {
                return Results.NotFound();
            }

            var rows = await endpoints.DeleteBookAsync(book, cancellationToken);
            return rows <= 0 ? Results.NotFound() : Results.NoContent();
        })
    .WithName("DeleteBook");

app.Run();