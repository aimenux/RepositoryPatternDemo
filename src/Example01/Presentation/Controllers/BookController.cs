using Example01.Domain;
using Example01.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Example01.Presentation.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class BookController : ControllerBase
{
    private readonly BookDbContext _context;
    private readonly ILogger<BookController> _logger;

    public BookController(BookDbContext context, ILogger<BookController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await _context.Books.ToListAsync(cancellationToken);
        return Ok(books);
    }
    
    [HttpGet("{bookId:int}")]
    [ActionName(nameof(GetBookByIdAsync))]
    public async Task<IActionResult> GetBookByIdAsync([FromRoute] int bookId, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { bookId }, cancellationToken: cancellationToken);
        return book is null ? NotFound() : Ok(book);
    }
        
    [HttpPost]
    public async Task<IActionResult> PostBookAsync([FromBody] Book book, CancellationToken cancellationToken)
    {
        await _context.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetBookByIdAsync), new { bookId = book.Id }, book);
    }
    
    [HttpPut("{bookId:int}")]
    public async Task<IActionResult> PutBookAsync([FromRoute] int bookId, [FromBody] Book book, CancellationToken cancellationToken)
    {
        if (bookId != book.Id)
        {
            return BadRequest();
        }
        
        _context.Entry(book).State = EntityState.Modified;
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows <= 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> DeleteBookAsync([FromRoute] int bookId, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FindAsync(new object[] { bookId }, cancellationToken: cancellationToken);
        if (book is null)
        {
            return NotFound();
        }
    
        _context.Books.Remove(book);
        var rows = await _context.SaveChangesAsync(cancellationToken);
        return rows <= 0 ? NotFound() : NoContent();
    }
}