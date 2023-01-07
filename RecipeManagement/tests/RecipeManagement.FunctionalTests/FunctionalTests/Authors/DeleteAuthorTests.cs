namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteAuthorTests : TestBase
{
    [Test]
    public async Task delete_author_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthor = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id).Generate());
        await InsertAsync(fakeAuthor);

        // Act
        var route = ApiRoutes.Authors.Delete(fakeAuthor.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}