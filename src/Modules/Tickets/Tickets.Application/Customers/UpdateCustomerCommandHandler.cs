using ErrorOr;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Customers;

namespace Tickets.Application.Customers;

public sealed record UpdateCustomerCommand(Guid CustomerId, string FirstName, string LastName) : IRequest<ErrorOr<Success>>;

internal sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ErrorOr<Success>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
            return CustomerErrors.CustomerIsNotFound(request.CustomerId);

        customer.Update(request.FirstName, request.LastName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

internal sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
    }
}
