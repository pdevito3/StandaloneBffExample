namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateAuthorTests : TestBase
{
    [Test]
    public async Task create_author_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthor = new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate();

        // Act
        var route = ApiRoutes.Authors.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAuthor);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}