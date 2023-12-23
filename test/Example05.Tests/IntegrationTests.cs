using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Example05.Domain;
using FluentAssertions;

namespace Example05.Tests;

public class IntegrationTests
{
    [Theory]
    [InlineData("api/books/1")]
    [InlineData("api/books/list")]
    public async Task Should_Get_Books_Returns_Success(string route)
    {
        // arrange
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.GetAsync(route);
        var responseBody = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Should_Post_Book_Returns_Success()
    {
        // arrange
        var book = new Book(0, "post-title", "post-author");
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.PostAsJsonAsync("api/books", book);
        var responseBody = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNullOrWhiteSpace();
    }
        
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Should_Put_Book_Returns_Success(int bookId)
    {
        // arrange
        var book = new Book(bookId, "put-title", "put-author");
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.PutAsJsonAsync($"api/books/{bookId}", book);
        var responseBody = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseBody.Should().NotBeNull().And.BeEmpty();
    }
    
    [Fact]
    public async Task Should_Delete_Book_Returns_Success()
    {
        // arrange
        var book = new Book(0, "post-title", "post-author");
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.PostAsJsonAsync("api/books", book);
        var bookId = await GetBookIdAsync(response);
        response = await client.DeleteAsync($"api/books/{bookId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseBody.Should().NotBeNull().And.BeEmpty();
    }

    private static async Task<int> GetBookIdAsync(HttpResponseMessage response)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var body = await response.Content.ReadAsStringAsync();
        var book = JsonSerializer.Deserialize<Book>(body, options);
        return book.Id;
    }
}