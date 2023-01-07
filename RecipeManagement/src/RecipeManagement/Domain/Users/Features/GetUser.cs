namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Users.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetUser
{
    public sealed class Query : IRequest<UserDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRepository userRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _heimGuard = heimGuard;
        }

        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadUsers);

            var result = await _userRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<UserDto>(result);
        }
    }
}