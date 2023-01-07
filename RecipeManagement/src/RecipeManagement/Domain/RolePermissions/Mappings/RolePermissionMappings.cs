namespace RecipeManagement.Domain.RolePermissions.Mappings;

using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain.RolePermissions;
using Mapster;

public sealed class RolePermissionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RolePermission, RolePermissionDto>();
        config.NewConfig<RolePermissionForCreationDto, RolePermission>()
            .TwoWays();
        config.NewConfig<RolePermissionForUpdateDto, RolePermission>()
            .TwoWays();
    }
}