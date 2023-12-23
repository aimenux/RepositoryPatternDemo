using Example07.Domain;
using Example07.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Example07.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IUnitOfWork unitOfWork, ILogger<BooksController> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await _unitOfWork.Books.GetAllAsync(cancellationToken);
        return Ok(books);
    }
    
    [HttpGet("{bookId:int}")]
    [ActionName(nameof(GetBookByIdAsync))]
    public async Task<IActionResult> GetBookByIdAsync([FromRoute] int bookId, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookId, cancellationToken);
        return book is null ? NotFound() : Ok(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> PostBookAsync([FromBody] Book book, CancellationToken cancellationToken)
    {
        await _unitOfWork.Books.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetBookByIdAsync), new { bookId = book.Id }, book);
    }
    
    [HttpPut("{bookId:int}")]
    public async Task<IActionResult> PutBookAsync([FromRoute] int bookId, [FromBody] Book book, CancellationToken cancellationToken)
    {
        if (bookId != book.Id)
        {
            return BadRequest();
        }
        
        _unitOfWork.Books.Update(book);
        var rows = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return rows <= 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> DeleteBookAsync([FromRoute] int bookId, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookId, cancellationToken);
        if (book is null)
        {
            return NotFound();
        }

        _unitOfWork.Books.Delete(book);
        var rows = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return rows <= 0 ? NotFound() : NoContent();
    }    
}