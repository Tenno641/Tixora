using ErrorOr;
using Events.Application.Common.Interfaces;
using Events.Domain.Categories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Events.Application.Categories;

public sealed record CreateCategoryCommand(string Name) : IRequest<ErrorOr<Guid>>;

internal sealed class CreateCategory: IRequestHandler<CreateCategoryCommand, ErrorOr<Guid>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateCategory(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = Category.Create(request.Name);

        _categoryRepository.Insert(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
    }
}

public class CategoryCreatedEventHandler: INotificationHandler<CategoryCreatedEvent>
{
    private readonly ILogger<CategoryCreatedEventHandler> _logger;
    
    public CategoryCreatedEventHandler(ILogger<CategoryCreatedEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("New Category");
        return Task.CompletedTask;
    }
}