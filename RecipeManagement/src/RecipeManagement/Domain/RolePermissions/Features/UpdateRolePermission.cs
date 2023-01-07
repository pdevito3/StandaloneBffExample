namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain.RolePermissions.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateRolePermission
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly RolePermissionForUpdateDto UpdatedRolePermissionData;

        public Command(Guid id, RolePermissionForUpdateDto updatedRolePermissionData)
        {
            Id = id;
            UpdatedRolePermissionData = updatedRolePermissionData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRolePermissionRepository rolePermissionRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateRolePermissions);

            var rolePermissionToUpdate = await _rolePermissionRepository.GetById(request.Id, cancellationToken: cancellationToken);

            rolePermissionToUpdate.Update(request.UpdatedRolePermissionData);
            _rolePermissionRepository.Update(rolePermissionToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}