using Example03.Domain;
using Example03.Infrastructure;
using Example03.Infrastructure.Repositories;
using Example03.Presentation.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Example03.Tests;

public class UnitTests
{
    [Fact]
    public async Task Should_Get_Books_Returns_Success()
    {
        // arrange
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BooksController>.Instance;
        var controller = new BooksController(repository, logger);

        // act
        var result = await controller.GetBooksAsync(CancellationToken.None);

        // assert
        result.Should().BeOfType<OkObjectResult>();
        result
            .As<OkObjectResult>().Value
            .As<IEnumerable<Book>>()
            .Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Should_Get_Book_By_Id_Returns_Success(int bookId)
    {
        // arrange
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BooksController>.Instance;
        var controller = new BooksController(repository, logger);

        // act
        var result = await controller.GetBookByIdAsync(bookId, CancellationToken.None);

        // assert
        result.Should().BeOfType<OkObjectResult>();
        result
            .As<OkObjectResult>().Value
            .As<Book>()
            .Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Post_Book_Returns_Success()
    {
        // arrange
        var book = new Book(0, "post-title", "post-author");
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BooksController>.Instance;
        var controller = new BooksController(repository, logger);

        // act
        var result = await controller.PostBookAsync(book, CancellationToken.None);

        // assert
        result.Should().BeOfType<CreatedAtActionResult>();
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
        var logger = NullLogger<BooksController>.Instance;
        var controller = new BooksController(repository, logger);

        // act
        var result = await controller.PutBookAsync(book.Id, book, CancellationToken.None);

        // assert
        result.Should().BeOfType<NoContentResult>();
    }
    
    [Fact]
    public async Task Should_Delete_Book_Returns_Success()
    {
        // arrange
        var book = new Book(0, "title", "author");
        await using var context = await BuildDbContextAsync();
        var repository = new BookRepository(context);
        var logger = NullLogger<BooksController>.Instance;
        var controller = new BooksController(repository, logger);

        // act
        var result1 = await controller.PostBookAsync(book, CancellationToken.None);
        var result2 = await controller.DeleteBookAsync(book.Id, CancellationToken.None);

        // assert
        result1.Should().BeOfType<CreatedAtActionResult>();
        result2.Should().BeOfType<NoContentResult>();
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