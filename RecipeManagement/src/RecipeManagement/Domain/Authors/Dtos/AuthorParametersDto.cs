namespace RecipeManagement.Domain.Authors.Dtos;

using SharedKernel.Dtos;

public sealed class AuthorParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
