using ErrorOr;
using Events.Application.Common;
using Events.Application.Common.Interfaces;
using Events.Domain.Categories;
using FluentValidation;
using MediatR;

namespace Events.Application.Categories;

public sealed record ArchiveCategoryCommand(Guid CategoryId) : IRequest<ErrorOr<Success>>;

public class ArchiveCategory : IRequestHandler<ArchiveCategoryCommand, ErrorOr<Success>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ArchiveCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(ArchiveCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return CategoryErrors.CategoryNotFound;

        if (category.IsArchived)
            return CategoryErrors.CategoryAlreadyArchived;

        category.Archive();
        
        _categoryRepository.Update(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

public sealed class ArchiveCategoryCommandValidator : AbstractValidator<ArchiveCategoryCommand>
{
    public ArchiveCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId).NotEmpty();
    }
}
