using Events.Domain;
using Events.Domain.Categories;

namespace Events.Application.Common;

public interface ICategoryRepository
{
    void Insert(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    void Update(Category category);
}