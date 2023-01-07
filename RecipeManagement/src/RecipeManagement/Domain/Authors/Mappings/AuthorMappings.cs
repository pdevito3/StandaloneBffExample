namespace RecipeManagement.Domain.Authors.Mappings;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors;
using Mapster;

public sealed class AuthorMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Author, AuthorDto>();
        config.NewConfig<AuthorForCreationDto, Author>()
            .TwoWays();
        config.NewConfig<AuthorForUpdateDto, Author>()
            .TwoWays();
    }
}