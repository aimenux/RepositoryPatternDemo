using Example04.Domain;
using Example04.Infrastructure;
using Example04.Infrastructure.Repositories;
using Example04.Presentation;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Example04.Tests;

public class UnitTests
{
    [Fact]
    public async Task Should_Get_Books_Returns_Success()
    {
        // arrange
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BookEndpoints>.Instance;
        var endpoints = new BookEndpoints(repository, logger);

        // act
        var books = await endpoints.GetBooksAsync(CancellationToken.None);

        // assert
        books.Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Should_Get_Book_By_Id_Returns_Success(int bookId)
    {
        // arrange
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BookEndpoints>.Instance;
        var endpoints = new BookEndpoints(repository, logger);

        // act
        var book = await endpoints.GetBookByIdAsync(bookId, CancellationToken.None);

        // assert
        book.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Should_Post_Book_Returns_Success()
    {
        // arrange
        var book = new Book(0, "post-title", "post-author");
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BookEndpoints>.Instance;
        var endpoints = new BookEndpoints(repository, logger);

        // act
        var rows = await endpoints.AddBookAsync(book, CancellationToken.None);

        // assert
        rows.Should().Be(1);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Should_Put_Book_Returns_Success(int bookId)
    {
        // arrange
        var book = new Book(bookId, "put-title", "put-author");
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BookEndpoints>.Instance;
        var endpoints = new BookEndpoints(repository, logger);

        // act
        var rows = await endpoints.UpdateBookAsync(book, CancellationToken.None);

        // assert
        rows.Should().Be(1);
    }
    
    [Fact]
    public async Task Should_Delete_Book_Returns_Success()
    {
        // arrange
        var book = new Book(0, "title", "author");
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BookEndpoints>.Instance;
        var endpoints = new BookEndpoints(repository, logger);

        // act
        var rows1 = await endpoints.AddBookAsync(book, CancellationToken.None);
        var rows2 = await endpoints.DeleteBookAsync(book, CancellationToken.None);

        // assert
        rows1.Should().Be(1);
        rows2.Should().Be(1);
    }    

    private static async Task<BookDbContext> BuildDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase("UnitTestsDB")
            .Options;
        var context = new BookDbContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        return context;
    }
}