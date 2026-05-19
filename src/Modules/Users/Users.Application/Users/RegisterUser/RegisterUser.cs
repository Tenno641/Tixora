using FluentValidation;
using MediatR;
using Users.Domain.Users;
using ErrorOr;
using Users.Application.Common;

namespace Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, string FirstName, string LastName): IRequest<ErrorOr<Guid>>;
    
internal sealed class RegisterUser : IRequestHandler<RegisterUserCommand, ErrorOr<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityProviderService _identityProviderService;
    
    public RegisterUser(IUserRepository userRepository, IUnitOfWork unitOfWork, IIdentityProviderService identityProviderService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _identityProviderService = identityProviderService;
    }
    
    public async Task<ErrorOr<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        UserModel userModel = new UserModel(request.FirstName, request.LastName, request.Email, request.Password);

        ErrorOr<string> result = await _identityProviderService.RegisterUserAsync(userModel, cancellationToken);

        if (result.IsError)
            return result.Errors;
            
        User user = User.Create(request.Email, request.FirstName, request.LastName, result.Value);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).EmailAddress();
        RuleFor(c => c.Password).MinimumLength(6);
    }
}