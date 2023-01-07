namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MediatR;

public static class DeleteRecipe
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteRecipes);

            var recordToDelete = await _recipeRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _recipeRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}