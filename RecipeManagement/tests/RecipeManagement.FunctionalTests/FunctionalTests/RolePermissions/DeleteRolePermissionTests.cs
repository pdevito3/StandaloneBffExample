namespace RecipeManagement.FunctionalTests.FunctionalTests.RolePermissions;

using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteRolePermissionTests : TestBase
{
    [Test]
    public async Task delete_rolepermission_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Delete(fakeRolePermission.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_rolepermission_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());

        // Act
        var route = ApiRoutes.RolePermissions.Delete(fakeRolePermission.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_rolepermission_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.RolePermissions.Delete(fakeRolePermission.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}