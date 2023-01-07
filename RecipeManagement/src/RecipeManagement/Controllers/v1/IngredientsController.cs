namespace RecipeManagement.Controllers.v1;

using RecipeManagement.Domain.Ingredients.Features;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/ingredients")]
[ApiVersion("1.0")]
public sealed class IngredientsController: ControllerBase
{
    private readonly IMediator _mediator;

    public IngredientsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Ingredients.
    /// </summary>
    [HttpGet(Name = "GetIngredients")]
    public async Task<IActionResult> GetIngredients([FromQuery] IngredientParametersDto ingredientParametersDto)
    {
        var query = new GetIngredientList.Query(ingredientParametersDto);
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
    /// Gets a single Ingredient by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetIngredient")]
    public async Task<ActionResult<IngredientDto>> GetIngredient(Guid id)
    {
        var query = new GetIngredient.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Ingredient record.
    /// </summary>
    [HttpPost(Name = "AddIngredient")]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody]IngredientForCreationDto ingredientForCreation)
    {
        var command = new AddIngredient.Command(ingredientForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetIngredient",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Ingredient.
    /// </summary>
    [HttpPut("{id:guid}", Name = "UpdateIngredient")]
    public async Task<IActionResult> UpdateIngredient(Guid id, IngredientForUpdateDto ingredient)
    {
        var command = new UpdateIngredient.Command(id, ingredient);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Ingredient record.
    /// </summary>
    [HttpDelete("{id:guid}", Name = "DeleteIngredient")]
    public async Task<ActionResult> DeleteIngredient(Guid id)
    {
        var command = new DeleteIngredient.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Creates one or more Ingredient records.
    /// </summary>
    [HttpPost("batch", Name = "AddIngredientList")]
    public async Task<ActionResult<IngredientDto>> AddIngredient([FromBody]IEnumerable<IngredientForCreationDto> ingredientForCreation,
        [FromQuery(Name = "recipeId"), BindRequired] Guid recipeId)
    {
        var command = new AddIngredientList.AddIngredientListCommand(ingredientForCreation, recipeId);
        var commandResponse = await _mediator.Send(command);

        return Created("GetIngredient", commandResponse);
    }

    // endpoint marker - do not delete this comment
}
