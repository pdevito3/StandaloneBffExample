namespace RecipeManagement.FunctionalTests.FunctionalTests.Users;

using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteUserTests : TestBase
{
    [Test]
    public async Task delete_user_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.Delete(fakeUser.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_user_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());

        // Act
        var route = ApiRoutes.Users.Delete(fakeUser.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_user_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Users.Delete(fakeUser.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}