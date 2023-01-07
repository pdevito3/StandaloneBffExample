namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Authors.Features;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/authors")]
[ApiVersion("1.0")]
public sealed class AuthorsController: ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Authors.
    /// </summary>
    [HttpGet(Name = "GetAuthors")]
    public async Task<IActionResult> GetAuthors([FromQuery] AuthorParametersDto authorParametersDto)
    {
        var query = new GetAuthorList.Query(authorParametersDto);
        var queryResponse = await _mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a single Author by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetAuthor")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(Guid id)
    {
        var query = new GetAuthor.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Author record.
    /// </summary>
    [HttpPost(Name = "AddAuthor")]
    public async Task<ActionResult<AuthorDto>> AddAuthor([FromBody]AuthorForCreationDto authorForCreation)
    {
        var command = new AddAuthor.Command(authorForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetAuthor",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Author.
    /// </summary>
    [HttpPut("{id:guid}", Name = "UpdateAuthor")]
    public async Task<IActionResult> UpdateAuthor(Guid id, AuthorForUpdateDto author)
    {
        var command = new UpdateAuthor.Command(id, author);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Author record.
    /// </summary>
    [HttpDelete("{id:guid}", Name = "DeleteAuthor")]
    public async Task<ActionResult> DeleteAuthor(Guid id)
    {
        var command = new DeleteAuthor.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
