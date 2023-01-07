namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetAuthorTests : TestBase
{
    [Test]
    public async Task get_author_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthor = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id).Generate());
        await InsertAsync(fakeAuthor);

        // Act
        var route = ApiRoutes.Authors.GetRecord(fakeAuthor.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}