using Example05.Domain;
using Example05.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Example05.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IGenericRepository<Book> _repository;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IGenericRepository<Book> repository, ILogger<BooksController> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await _repository.GetAllAsync(cancellationToken);
        return Ok(books);
    }
    
    [HttpGet("{bookId:int}")]
    [ActionName(nameof(GetBookByIdAsync))]
    public async Task<IActionResult> GetBookByIdAsync([FromRoute] int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(bookId, cancellationToken);
        return book is null ? NotFound() : Ok(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> PostBookAsync([FromBody] Book book, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(book, cancellationToken);
        return CreatedAtAction(nameof(GetBookByIdAsync), new { bookId = book.Id }, book);
    }
    
    [HttpPut("{bookId:int}")]
    public async Task<IActionResult> PutBookAsync([FromRoute] int bookId, [FromBody] Book book, CancellationToken cancellationToken)
    {
        if (bookId != book.Id)
        {
            return BadRequest();
        }
        
        var rows = await _repository.UpdateAsync(book, cancellationToken);
        return rows <= 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> DeleteBookAsync([FromRoute] int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(bookId, cancellationToken);
        if (book is null)
        {
            return NotFound();
        }

        var rows = await _repository.DeleteAsync(book, cancellationToken);
        return rows <= 0 ? NotFound() : NoContent();
    }    
}