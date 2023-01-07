namespace RecipeManagement.SharedTestHelpers.Fakes.Author;

using AutoBogus;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;

public sealed class FakeAuthor
{
    public static Author Generate(AuthorForCreationDto authorForCreationDto)
    {
        return Author.Create(authorForCreationDto);
    }

    public static Author Generate()
    {
        return Generate(new FakeAuthorForCreationDto().Generate());
    }
}