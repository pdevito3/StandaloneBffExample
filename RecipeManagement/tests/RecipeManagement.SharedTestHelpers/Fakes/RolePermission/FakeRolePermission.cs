namespace RecipeManagement.SharedTestHelpers.Fakes.RolePermission;

using AutoBogus;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;

public sealed class FakeRolePermission
{
    public static RolePermission Generate(RolePermissionForCreationDto rolePermissionForCreationDto)
    {
        return RolePermission.Create(rolePermissionForCreationDto);
    }

    public static RolePermission Generate()
    {
        return Generate(new FakeRolePermissionForCreationDto().Generate());
    }
}