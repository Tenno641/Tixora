using ErrorOr;
using FluentValidation;
using MediatR;
using Users.Application.Common;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : IRequest<ErrorOr<Success>>;

internal sealed class UpdateUser : IRequestHandler<UpdateUserCommand, ErrorOr<Success>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUser(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetAsync(request.UserId, cancellationToken);

        if (user is null)
            return UserErrors.UserIsNotFound(request.UserId);

        user.Update(request.FirstName, request.LastName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
    }
}
