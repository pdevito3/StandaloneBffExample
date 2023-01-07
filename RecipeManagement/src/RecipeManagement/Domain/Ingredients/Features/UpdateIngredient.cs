namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateIngredient
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly IngredientForUpdateDto UpdatedIngredientData;

        public Command(Guid id, IngredientForUpdateDto updatedIngredientData)
        {
            Id = id;
            UpdatedIngredientData = updatedIngredientData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IIngredientRepository ingredientRepository, IUnitOfWork unitOfWork)
        {
            _ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var ingredientToUpdate = await _ingredientRepository.GetById(request.Id, cancellationToken: cancellationToken);

            ingredientToUpdate.Update(request.UpdatedIngredientData);
            _ingredientRepository.Update(ingredientToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}