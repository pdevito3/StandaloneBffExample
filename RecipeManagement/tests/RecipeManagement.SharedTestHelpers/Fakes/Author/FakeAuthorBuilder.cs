namespace RecipeManagement.SharedTestHelpers.Fakes.Author;

using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;

public class FakeAuthorBuilder
{
    private AuthorForCreationDto _creationData = new FakeAuthorForCreationDto().Generate();

    public FakeAuthorBuilder WithDto(AuthorForCreationDto dto)
    {
        _creationData = dto;
        return this;
    }
    
    public Author Build()
    {
        var result = Author.Create(_creationData);
        return result;
    }
}