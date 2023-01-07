namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using MediatR;

public static class DeleteIngredient
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
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IIngredientRepository ingredientRepository, IUnitOfWork unitOfWork)
        {
            _ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await _ingredientRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _ingredientRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}