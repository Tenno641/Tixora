using Events.Domain.Categories;

namespace Events.Application.Common.Interfaces;

public interface ICategoryRepository
{
    void Insert(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    void Update(Category category);
}