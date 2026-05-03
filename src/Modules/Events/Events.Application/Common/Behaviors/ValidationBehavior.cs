using ErrorOr;
using FluentValidation;
using MediatR;

namespace Events.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            await next(cancellationToken);

        var validationResult = _validators.Select(validator => validator.Validate(request));

        var failures = validationResult
            .SelectMany(vr => vr.Errors)
            .Select(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        return (dynamic)failures;
    }
}